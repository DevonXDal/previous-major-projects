import 'package:devonium_weather/json/general_weather.dart';
import 'package:devonium_weather/json/solar_lunar_information.dart';

class LocationData {
  GeneralWeather relatedWeatherInformation;
  SolarLunarInformation relatedSolarLunarInformation;

  DateTime lastWeatherUpdate;
  DateTime lastSolarLunarUpdate;

  LocationData(this.relatedWeatherInformation, this.relatedSolarLunarInformation, this.lastWeatherUpdate, this.lastSolarLunarUpdate);
}
