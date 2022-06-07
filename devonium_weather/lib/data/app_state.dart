import 'package:devonium_weather/data/location_data.dart';
import 'package:devonium_weather/helper/unit_type.dart';
import 'package:devonium_weather/provider/location_change_notifier.dart';
import 'package:flutter/cupertino.dart';
import 'package:provider/provider.dart';

import 'location_mapping.dart';

class AppState {
  static Map<LocationMapping, LocationData> locations = {
    currentLocation: LocationData(null, null, null, null),
  };
  //Change to test deployment To
  static LocationMapping currentLocation = LocationMapping('Parkersburg, West Virginia, United States', 39.2661, -81.5422);

  static UnitType settingSetForTemperature = UnitType.metric;
  static UnitType settingSetForSpeed = UnitType.metric;
  static UnitType settingSetForDistance = UnitType.metric;

  static void changeLocationHelper(BuildContext context, String newValue) {
    LocationChangeNotifier locationHeaderCN = context.read<LocationChangeNotifier>();
    locationHeaderCN.setWeatherFor(newValue);
  }
}
