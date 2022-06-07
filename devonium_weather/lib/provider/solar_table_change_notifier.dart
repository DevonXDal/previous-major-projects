import 'package:devonium_weather/json/solar_lunar_information.dart';
import 'package:devonium_weather/repository/weather_repository.dart';
import 'package:flutter/material.dart';

class SolarTableChangeNotifier extends ChangeNotifier {
  final WeatherRepository _repository;
  List<String> _solarData;

  List<String> get solarData => _solarData;

  SolarTableChangeNotifier(this._repository);

  void updateSolarInformation() async {
    SolarLunarInformation solarInformation = await _repository.getSolarLunar();

    _solarData = _getFormattedSolarTableInformation(solarInformation);
    notifyListeners();
  }

  List<String> _getFormattedSolarTableInformation(SolarLunarInformation solarLunarInformation) {
    Time solarTime = solarLunarInformation.location.time.first;
    int offsetHours = DateTime.now().timeZoneOffset.inHours;

    DateTime sunrise = DateTime.parse(solarTime.sunrise.time.toString()).add(Duration(hours: offsetHours));
    DateTime sunset = DateTime.parse(solarTime.sunset.time.toString()).add(Duration(hours: offsetHours));
    DateTime solarNoon = DateTime.parse(solarTime.solarnoon.time.toString()).add(Duration(hours: offsetHours));
    DateTime solarMidnight = DateTime.parse(solarTime.solarmidnight.time.toString()).add(Duration(hours: offsetHours));

    List<String> data = [
      _getHourMinute(sunrise),
      _getHourMinute(sunset),
      _getHourMinute(solarNoon),
      _getHourMinute(solarMidnight),
      _getHoursOfDaylight(sunrise, sunset).toString(),
      _getHoursOfDark(sunrise, sunset).toString()
    ];

    return data;
  }

  String _getHourMinute(DateTime dateTime) {
    String formattedHour = (dateTime.hour.abs() < 10) ? '0${dateTime.hour.abs()}' : dateTime.hour.abs().toString();
    String formattedMinute = (dateTime.minute.abs() < 10) ? '0${dateTime.minute.abs()}' : dateTime.minute.abs().toString();

    return '$formattedHour:$formattedMinute';
  }

  int _getHoursOfDaylight(DateTime sunrise, DateTime sunset) {
    return sunset.hour - sunrise.hour;
  }

  int _getHoursOfDark(DateTime sunrise, DateTime sunset) {
    return (24 - sunset.hour) + sunrise.hour;
  }
}
