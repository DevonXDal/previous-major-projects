import 'package:devonium_weather/json/solar_lunar_information.dart';
import 'package:devonium_weather/repository/weather_repository.dart';
import 'package:flutter/material.dart';

class LunarTableChangeNotifier extends ChangeNotifier {
  final WeatherRepository _repository;
  List<String> _lunarData;

  List<String> get lunarData => _lunarData;

  LunarTableChangeNotifier(this._repository);

  void updateLunarInformation() async {
    SolarLunarInformation lunarInformation = await _repository.getSolarLunar();

    _lunarData = _getFormattedLunarTableInformation(lunarInformation);
    notifyListeners();
  }

  List<String> _getFormattedLunarTableInformation(SolarLunarInformation solarLunarInformation) {
    //https://stackoverflow.com/questions/15900254/dart-how-to-match-and-then-replace-a-regexp

    Time lunarTime = solarLunarInformation.location.time.first;
    int offsetHours = DateTime.now().timeZoneOffset.inHours;

    //https://stackoverflow.com/questions/6734521/getting-a-double-out-of-a-string
    String moonPhase = lunarTime.moonphase.desc;
    RegExp reg = RegExp('(?!=\\d\\.\\d\\.)([\\d.]+)');
    RegExpMatch match = reg.firstMatch(moonPhase);
    String percentFull = (match == null) ? '0.50' : moonPhase.substring(match.start, match.end);
    String phase = '';
    bool goingToFullMoon = true;

    if (moonPhase.contains('waning gibbous')) {
      phase = 'WaningG';
      goingToFullMoon = false;
    } else if (moonPhase.contains('waxing gibbous')) {
      phase = 'WaxingG';
    } else if (moonPhase.contains('waning crescent')) {
      phase = 'WaningC';
      goingToFullMoon = false;
    } else if (moonPhase.contains('waxing crescent')) {
      phase = 'WaxingC';
    } else if (moonPhase.contains('full')) {
      phase = 'FullMoon';
      goingToFullMoon = false;
    } else {
      //New Moon
      phase = 'NewMoon';
    }

    DateTime moonRise = DateTime.parse(lunarTime.sunset.time.toString()).add(Duration(hours: offsetHours));
    DateTime lunarNoon = DateTime.parse(lunarTime.highMoon.time.toString()).add(Duration(hours: offsetHours));
    DateTime moonSet = DateTime.parse(lunarTime.moonset.time.toString()).add(Duration(hours: offsetHours));
    DateTime lunarMidnight = DateTime.parse(lunarTime.lowMoon.time.toString()).add(Duration(hours: offsetHours));

    String value = lunarTime.moonphase.desc.replaceAllMapped('/[0-9]+\.[0-9]+/g', (match) {
      return '${match.group(0)}';
    });

    List<String> data = [
      phase,
      _getHourMinute(moonRise),
      _getHourMinute(lunarNoon),
      _getHourMinute(moonSet),
      _getHourMinute(lunarMidnight),
      _daysUntilFullMoon(double.parse(percentFull), goingToFullMoon).toString(),
      _daysUntilNewMoon(double.parse(percentFull), goingToFullMoon).toString()
    ];

    return data;
  }

  String _getHourMinute(DateTime dateTime) {
    String formattedHour = (dateTime.hour.abs() < 10) ? '0${dateTime.hour.abs()}' : dateTime.hour.abs().toString();
    String formattedMinute = (dateTime.minute.abs() < 10) ? '0${dateTime.minute.abs()}' : dateTime.minute.abs().toString();

    return '$formattedHour:$formattedMinute';
  }

  int _daysUntilFullMoon(double currentState, bool movingTowardsFull) {
    double dailyChange = (movingTowardsFull) ? 7.352941176 : -7.352941176;
    double value = currentState;
    int days = 0;

    while (value < 96) {
      value += dailyChange;

      if (value < 4) {
        dailyChange *= -1;
      }
      days++;
    }

    return days;
  }

  int _daysUntilNewMoon(double currentState, bool movingTowardsFull) {
    double dailyChange = (movingTowardsFull) ? 7.352941176 : -7.352941176; //3.676470588 : -3.676470588;
    double value = currentState;
    int days = 0;

    while (value > 4) {
      value += dailyChange;

      if (value > 96) {
        dailyChange *= -1;
      }
      days++;
    }

    return days;
  }
}
