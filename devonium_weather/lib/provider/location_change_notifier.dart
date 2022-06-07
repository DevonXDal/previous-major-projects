import 'package:devonium_weather/data/location_header_data.dart';
import 'package:devonium_weather/json/geocoding_reponse.dart';
import 'package:devonium_weather/repository/app_state_repository.dart';
import 'package:devonium_weather/repository/geocoding_repository.dart';
import 'package:flutter/material.dart';

class LocationChangeNotifier extends ChangeNotifier {
  final AppStateRepository _stateRepository;
  final GeocodingRepository _geocodingRepository;
  LocationHeaderData _locationData;

  LocationHeaderData get locationData => _locationData;

  LocationChangeNotifier(this._stateRepository, this._geocodingRepository) {}

  Future<void> addLocation(String city, String state, String country) async {
    country = country.substring(6).trim();
    if (state.isNotEmpty) {
      state = '$state,';
    }
    if (city.isNotEmpty) {
      city = '$city,';
    }
    GeocodingResponse locationCoordinates = await _geocodingRepository.findCoordinates(city, state, country);
    _stateRepository.addLocation('$city $state $country'.trim(), locationCoordinates.lat, locationCoordinates.lon);
    refresh();
  }

  void setWeatherFor(String name) {
    _stateRepository.switchLocation(name);
    refresh();
  }

  void removeLocation(String name) {
    _stateRepository.removeLocation(name);
    refresh();
  }

  void refresh() async {
    _locationData = _stateRepository.getUpdatedLocationData();
    notifyListeners();
  }

  @override
  void dispose() {
    super.dispose();
  }
}
