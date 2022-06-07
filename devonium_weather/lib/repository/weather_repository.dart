import 'dart:convert';

import 'package:devonium_weather/data/app_state.dart';
import 'package:devonium_weather/data/location_data.dart';
import 'package:devonium_weather/data/location_mapping.dart';
import 'package:devonium_weather/json/general_weather.dart';
import 'package:devonium_weather/json/solar_lunar_information.dart';
import 'package:http/http.dart' as http;

class WeatherRepository {
  Future<GeneralWeather> getWeather() async {
    LocationData data = AppState.locations[AppState.currentLocation];
    LocationMapping locationInfo = AppState.currentLocation;

    if (data.lastWeatherUpdate != null && data.lastWeatherUpdate.add(Duration(hours: 1)).isAfter(DateTime.now()) && data.relatedWeatherInformation != null) {
      return data.relatedWeatherInformation;
    }
    DateTime _backupOfPreviousTime = data.lastWeatherUpdate;
    data.lastWeatherUpdate = DateTime.now();

    final response =
        await http.get(Uri.parse('https://api.openweathermap.org/data/2.5/onecall?lat=${locationInfo.latitude}&lon=${locationInfo.longitude}&units=metric&appid=b80446bb6672b0a8afc373654bce21a5'));

    if (response.statusCode == 200) {
      data.relatedWeatherInformation = GeneralWeather.fromJson(jsonDecode(response.body));
      return data.relatedWeatherInformation;
    } else {
      data.lastWeatherUpdate = _backupOfPreviousTime;
      throw Exception('Failed to retrieve weather information from OpenWeatherAPI');
    }
  }

  Future<SolarLunarInformation> getSolarLunar() async {
    LocationData data = AppState.locations[AppState.currentLocation];
    LocationMapping locationInfo = AppState.currentLocation;
    DateTime now = DateTime.now();

    if (data.lastSolarLunarUpdate != null && data.lastSolarLunarUpdate.add(Duration(hours: 1)).isAfter(DateTime.now()) && data.relatedSolarLunarInformation != null) {
      return data.relatedSolarLunarInformation;
    }

    DateTime _backupOfPreviousTime = data.lastSolarLunarUpdate;
    data.lastSolarLunarUpdate = now;

    Duration offset = now.timeZoneOffset;
    String offsetText = (offset.inHours.abs() < 10) ? '0${offset.inHours.abs()}' : offset.inHours.abs().toString();
    offsetText = (offset.inHours.isNegative) ? '-$offsetText:00' : '$offsetText:00';

    String formattedMonth = (now.month < 10) ? '0${now.month}' : now.month.toString();
    String formattedDay = (now.day < 10) ? '0${now.day}' : now.day.toString();

    String dateFormatted = '${now.year}-$formattedMonth-$formattedDay';

    final response = await http.get(Uri.parse(
        'https://api.met.no/weatherapi/sunrise/2.0/.json?date=$dateFormatted&lat=${locationInfo.latitude.toStringAsFixed(4)}&lon=${locationInfo.longitude.toStringAsFixed(4)}&offset=+00:00'));

    if (response.statusCode == 200) {
      return SolarLunarInformation.fromJson(jsonDecode(response.body));
    } else {
      data.lastSolarLunarUpdate = _backupOfPreviousTime;
      throw Exception('Failed to retrieve solar/lunar information from api.met.no');
    }
  }
}
