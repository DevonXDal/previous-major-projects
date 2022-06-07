import 'package:devonium_weather/data/app_state.dart';
import 'package:devonium_weather/helper/unit_converter.dart';
import 'package:devonium_weather/helper/unit_type.dart';
import 'package:devonium_weather/json/general_weather.dart';
import 'package:devonium_weather/repository/weather_repository.dart';
import 'package:flutter/material.dart';

class DailyChangeNotifier extends ChangeNotifier {
  final WeatherRepository _repository;
  List<List<String>> _dailyInformation;
  List<List<num>> _tempChartInformation;
  List<num> _popChartInformation;

  List<List<num>> get tempChartInformation => _tempChartInformation;

  List<num> get popChartInformation => _popChartInformation;

  List<List<String>> get dailyInformation => _dailyInformation;

  DailyChangeNotifier(this._repository);

  void updateWeatherInformation() async {
    GeneralWeather weather = await _repository.getWeather();

    _dailyInformation = _getFormattedDailyTableInformation(weather);
    _tempChartInformation = _getFormattedDailyTempChartInformation(weather);
    _popChartInformation = _getFormattedDailyPoPChartInformation(weather);
    notifyListeners();
  }

  List<List<String>> _getFormattedDailyTableInformation(GeneralWeather weather) {
    List<List<String>> dailyInformation = <List<String>>[]; //Row and then column
    List<String> tempRow = [];
    List<String> feelsLikeRow = [];
    List<String> popRow = [];
    List<String> humidityRow = [];
    List<String> dewPointRow = [];
    List<String> cloudsRow = [];
    List<String> visibilityRow = [];
    List<String> windSpeedRow = [];
    List<String> windDirectionRow = [];

    int currentDayIndex = _getCurrentDayIndex(weather);

    dailyInformation.add(tempRow);
    dailyInformation.add(feelsLikeRow);
    dailyInformation.add(popRow);
    dailyInformation.add(humidityRow);
    dailyInformation.add(dewPointRow);
    dailyInformation.add(cloudsRow);
    dailyInformation.add(visibilityRow);
    dailyInformation.add(windSpeedRow);
    dailyInformation.add(windDirectionRow);

    for (int i = 0; i < 5; i++) {
      Daily day = weather.daily.elementAt(currentDayIndex + i);
      int formattedPop = (day.pop * 100).round();
      int formattedHumidity = (day.humidity).round();

      String highTemp = (AppState.settingSetForTemperature == UnitType.metric) ? day.temp.max.round().toString() : UnitConverter.convertCelsiusToFahrenheit(day.temp.max).toString();
      String lowTemp = (AppState.settingSetForTemperature == UnitType.metric) ? UnitConverter.getFormattedCelsius(day.temp.min) : UnitConverter.getFormattedFahrenheit(day.temp.min);
      String feelsLikeMax = (AppState.settingSetForTemperature == UnitType.metric) ? day.feelsLike.day.round().toString() : UnitConverter.convertCelsiusToFahrenheit(day.feelsLike.day).toString();
      String feelsLikeMin = (AppState.settingSetForTemperature == UnitType.metric) ? UnitConverter.getFormattedCelsius(day.feelsLike.night) : UnitConverter.getFormattedFahrenheit(day.feelsLike.night);
      String dewPoint = (AppState.settingSetForTemperature == UnitType.metric) ? UnitConverter.getFormattedCelsius(day.dewPoint) : UnitConverter.getFormattedFahrenheit(day.dewPoint);
      String windSpeed = (AppState.settingSetForSpeed == UnitType.metric) ? UnitConverter.getFormattedMetersPerSecond(day.windSpeed) : UnitConverter.getFormattedMilesPerHour(day.windSpeed);

      tempRow.add('$highTemp/$lowTemp');
      feelsLikeRow.add('$feelsLikeMax/$feelsLikeMin');
      popRow.add('$formattedPop%');
      humidityRow.add('$formattedHumidity%');
      dewPointRow.add(dewPoint);
      cloudsRow.add(day.clouds.toString() + '%');
      visibilityRow.add('N/A');
      windSpeedRow.add(windSpeed);
      windDirectionRow.add(UnitConverter.getFormattedWindDirection(day.windDeg));
    }

    return dailyInformation;
  }

  List<List<num>> _getFormattedDailyTempChartInformation(GeneralWeather weather) {
    List<List<num>> dailyInformation = <List<num>>[]; //Row and then column
    List<num> highTemp = [];
    List<num> lowTemp = [];

    int currentDayIndex = _getCurrentDayIndex(weather);

    dailyInformation.add(highTemp);
    dailyInformation.add(lowTemp);

    for (int i = 0; i < 5; i++) {
      Daily day = weather.daily.elementAt(currentDayIndex + i);

      int highTempFormatted = (AppState.settingSetForTemperature == UnitType.metric) ? day.temp.max.round() : UnitConverter.convertCelsiusToFahrenheit(day.temp.max);
      int lowTempFormatted = (AppState.settingSetForTemperature == UnitType.metric) ? day.temp.min.round() : UnitConverter.convertCelsiusToFahrenheit(day.temp.min);

      highTemp.add(highTempFormatted);
      lowTemp.add(lowTempFormatted);
    }

    return dailyInformation;
  }

  List<num> _getFormattedDailyPoPChartInformation(GeneralWeather weather) {
    List<num> popInformation = []; //Row and then column

    int currentDayIndex = _getCurrentDayIndex(weather);

    for (int i = 0; i < 5; i++) {
      Daily day = weather.daily.elementAt(currentDayIndex + i);

      popInformation.add((day.pop * 100).round());
    }

    return popInformation;
  }

  int _getCurrentDayIndex(GeneralWeather weather) {
    int currentDayIndex = 0;
    for (int index = 1; index < weather.daily.length; index++) {
      int differenceFromCurrent = (weather.current.dt - weather.daily.elementAt(index).dt).abs();
      int selectedDifferenceFromCurrent = (weather.current.dt - weather.daily.elementAt(currentDayIndex).dt).abs();

      if (differenceFromCurrent < selectedDifferenceFromCurrent) {
        currentDayIndex = index;
      }
    }

    return currentDayIndex;
  }
}
