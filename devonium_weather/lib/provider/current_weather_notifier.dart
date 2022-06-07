import 'package:devonium_weather/data/current_data.dart';
import 'package:devonium_weather/json/general_weather.dart';
import 'package:devonium_weather/repository/weather_repository.dart';
import 'package:flutter/material.dart';

class CurrentWeatherNotifier extends ChangeNotifier {
  final WeatherRepository _repository;
  CurrentData _currentData;

  CurrentData get currentData => _currentData;

  CurrentWeatherNotifier(this._repository);

  void updateWeatherInformation() async {
    GeneralWeather weather = await _repository.getWeather();

    _currentData = _getCurrentData(weather);
    notifyListeners();
  }

  CurrentData _getCurrentData(GeneralWeather weather) {
    String icon = weather.current.weather.first.icon;
    String temp = weather.current.temp.round().toString();

    return CurrentData(icon, temp);
  }
}
