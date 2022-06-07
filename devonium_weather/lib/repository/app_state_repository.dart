import 'package:devonium_weather/data/app_state.dart';
import 'package:devonium_weather/data/location_data.dart';
import 'package:devonium_weather/data/location_header_data.dart';
import 'package:devonium_weather/data/location_mapping.dart';

class AppStateRepository {
  void addLocation(String name, double latitude, double longitude) async {
    AppState.locations[LocationMapping(name, latitude, longitude)] = LocationData(null, null, null, null);
  }

  void switchLocation(String name) {
    LocationMapping newLocation;

    for (LocationMapping location in AppState.locations.keys) {
      if (location.locationName == name) {
        newLocation = location;
        break;
      }
    }

    AppState.currentLocation = newLocation;
  }

  void removeLocation(String name) {
    LocationMapping locationToRemove;

    for (LocationMapping location in AppState.locations.keys) {
      if (location.locationName == name) {
        locationToRemove = location;
        break;
      }
    }

    AppState.locations.remove(locationToRemove);

    if (AppState.currentLocation == locationToRemove) {
      AppState.currentLocation = AppState.locations.keys.first;
    }
  }

  LocationHeaderData getUpdatedLocationData() {
    String current = AppState.currentLocation.locationName;
    List<String> all = [];
    AppState.locations.keys.forEach((element) {
      all.add(element.locationName);
    });

    return LocationHeaderData(current, all);
  }
}
