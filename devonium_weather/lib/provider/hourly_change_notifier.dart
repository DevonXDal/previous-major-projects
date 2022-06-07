import 'package:devonium_weather/data/app_state.dart';
import 'package:devonium_weather/helper/unit_converter.dart';
import 'package:devonium_weather/helper/unit_type.dart';
import 'package:devonium_weather/json/general_weather.dart';
import 'package:devonium_weather/repository/weather_repository.dart';
import 'package:flutter/cupertino.dart';

class HourlyChangeNotifier extends ChangeNotifier {
  final WeatherRepository _repository;
  List<List<String>> _hourlyInformation;
  List<List<num>> _tempChartInformation;
  List<num> _popChartInformation;

  List<List<num>> get tempChartInformation => _tempChartInformation;

  List<num> get popChartInformation => _popChartInformation;

  List<List<String>> get hourlyInformation => _hourlyInformation;

  HourlyChangeNotifier(this._repository);

  void updateWeatherInformation() async {
    GeneralWeather weather = await _repository.getWeather();

    _hourlyInformation = _getFormattedHourlyTableInformation(weather);
    _tempChartInformation = _getFormattedHourlyTempChartInformation(weather);
    _popChartInformation = _getFormattedDailyPoPChartInformation(weather);
    notifyListeners();
  }

  List<List<String>> _getFormattedHourlyTableInformation(GeneralWeather weather) {
    List<List<String>> hourlyInformation = <List<String>>[]; //Row and then column
    List<String> tempRow = [];
    List<String> feelsLikeRow = [];
    List<String> popRow = [];
    List<String> humidityRow = [];
    List<String> dewPointRow = [];
    List<String> cloudsRow = [];
    List<String> visibilityRow = [];
    List<String> windSpeedRow = [];
    List<String> windDirectionRow = [];
    int currentHourIndex = _getCurrentHourIndex(weather);

    hourlyInformation.add(tempRow);
    hourlyInformation.add(feelsLikeRow);
    hourlyInformation.add(popRow);
    hourlyInformation.add(humidityRow);
    hourlyInformation.add(dewPointRow);
    hourlyInformation.add(cloudsRow);
    hourlyInformation.add(visibilityRow);
    hourlyInformation.add(windSpeedRow);
    hourlyInformation.add(windDirectionRow);

    for (int i = 0; i < 5; i++) {
      Hourly hour = weather.hourly.elementAt(currentHourIndex + i);
      int formattedPop = (hour.pop * 100).round();
      int formattedHumidity = (hour.humidity).round();
      num visibleKilometers = (hour.visibility / 1000);

      String temp = (AppState.settingSetForTemperature == UnitType.metric) ? UnitConverter.getFormattedCelsius(hour.temp) : UnitConverter.getFormattedFahrenheit(hour.temp);
      String feelsLike = (AppState.settingSetForTemperature == UnitType.metric) ? UnitConverter.getFormattedCelsius(hour.feelsLike) : UnitConverter.getFormattedFahrenheit(hour.feelsLike);
      String dewPoint = (AppState.settingSetForTemperature == UnitType.metric) ? UnitConverter.getFormattedCelsius(hour.dewPoint) : UnitConverter.getFormattedFahrenheit(hour.dewPoint);
      String windSpeed = (AppState.settingSetForSpeed == UnitType.metric) ? UnitConverter.getFormattedMetersPerSecond(hour.windSpeed) : UnitConverter.getFormattedMilesPerHour(hour.windSpeed);
      String visibility = (AppState.settingSetForDistance == UnitType.metric) ? UnitConverter.getFormattedKilometers(visibleKilometers) : UnitConverter.getFormattedMiles(visibleKilometers);

      tempRow.add(temp);
      feelsLikeRow.add(feelsLike);
      popRow.add('$formattedPop%');
      humidityRow.add('$formattedHumidity%');
      dewPointRow.add(dewPoint);
      cloudsRow.add(hour.clouds.toString() + '%');
      visibilityRow.add(visibility);
      windSpeedRow.add(windSpeed);
      windDirectionRow.add(UnitConverter.getFormattedWindDirection(hour.windDeg));
    }

    return hourlyInformation;
  }

  List<List<num>> _getFormattedHourlyTempChartInformation(GeneralWeather weather) {
    List<List<num>> hourlyInformation = <List<num>>[]; //Row and then column
    List<num> actualTemp = [];
    List<num> feelsLike = [];

    int currentHourIndex = _getCurrentHourIndex(weather);

    hourlyInformation.add(actualTemp);
    hourlyInformation.add(feelsLike);

    for (int i = 0; i < 5; i++) {
      Hourly hourly = weather.hourly.elementAt(currentHourIndex + i);

      int actualTempFormatted = (AppState.settingSetForTemperature == UnitType.metric) ? hourly.temp.round() : UnitConverter.convertCelsiusToFahrenheit(hourly.temp);
      int feelsLikeFormatted = (AppState.settingSetForTemperature == UnitType.metric) ? hourly.feelsLike.round() : UnitConverter.convertCelsiusToFahrenheit(hourly.feelsLike);

      actualTemp.add(actualTempFormatted);
      feelsLike.add(feelsLikeFormatted);
    }

    return hourlyInformation;
  }

  List<num> _getFormattedDailyPoPChartInformation(GeneralWeather weather) {
    List<num> popInformation = []; //Row and then column

    int currentDayIndex = _getCurrentHourIndex(weather);

    for (int i = 0; i < 5; i++) {
      Hourly hourly = weather.hourly.elementAt(currentDayIndex + i);

      popInformation.add((hourly.pop * 100).round());
    }

    return popInformation;
  }

  int _getCurrentHourIndex(GeneralWeather weather) {
    int currentHourIndex = 0;
    for (int index = 1; index < weather.hourly.length; index++) {
      int differenceFromCurrent = (weather.current.dt - weather.hourly.elementAt(index).dt).abs();
      int selectedDifferenceFromCurrent = (weather.current.dt - weather.hourly.elementAt(currentHourIndex).dt).abs();

      if (differenceFromCurrent < selectedDifferenceFromCurrent) {
        currentHourIndex = index;
      }
    }

    return currentHourIndex;
  }
}
