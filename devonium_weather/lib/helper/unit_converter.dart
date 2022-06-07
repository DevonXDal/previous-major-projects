class UnitConverter {
  static String getFormattedWindDirection(int degrees) {
    if (degrees >= 349 || degrees < 11) return 'N';
    if (degrees < 33) return 'NNE';
    if (degrees < 56) return 'NE';
    if (degrees < 78) return 'ENE';
    if (degrees < 101) return 'E';
    if (degrees < 123) return 'ESE';
    if (degrees < 146) return 'SE';
    if (degrees < 168) return 'SSE';
    if (degrees < 191) return 'S';
    if (degrees < 213) return 'SSW';
    if (degrees < 236) return 'SW';
    if (degrees < 258) return 'WSW';
    if (degrees < 281) return 'W';
    if (degrees < 303) return 'WNW';
    if (degrees < 326) return 'NW';
    if (degrees <= 348) return 'NNW';

    return 'ERROR'; //Number out of range
  }

  static int convertCelsiusToFahrenheit(num metricTemp) {
    return ((metricTemp * 1.8) + 32).round();
  }

  static int convertMetersPerSecondToMPH(num metricSpeed) {
    return (metricSpeed * 2.2369).round();
  }

  static int convertKilometersToMiles(num metricDistance) {
    return (metricDistance * .621371192).round();
  }

  static String getFormattedCelsius(num metricTemp) {
    return '${metricTemp.round()} °C';
  }

  static String getFormattedFahrenheit(num metricTemp) {
    return '${convertCelsiusToFahrenheit(metricTemp)} °F';
  }

  static String getFormattedMetersPerSecond(num metricSpeed) {
    return '${metricSpeed.round()} m/s';
  }

  static String getFormattedMilesPerHour(num metricSpeed) {
    return '${convertMetersPerSecondToMPH(metricSpeed)} mph';
  }

  static String getFormattedKilometers(num metricDistance) {
    return '${metricDistance.round()} km';
  }

  static String getFormattedMiles(num metricDistance) {
    return '${convertKilometersToMiles(metricDistance)} mi';
  }
}
