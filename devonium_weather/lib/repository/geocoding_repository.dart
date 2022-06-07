import 'dart:convert';

import 'package:devonium_weather/json/geocoding_reponse.dart';
import 'package:http/http.dart' as http;

class GeocodingRepository {
  const GeocodingRepository();

  Future<GeocodingResponse> findCoordinates(String city, String state, String country) async {
    final response = await http.get(Uri.parse('http://api.openweathermap.org/geo/1.0/direct?q=$city$state$country&limit=1&appid=b80446bb6672b0a8afc373654bce21a5'));

    if (response.statusCode == 200) {
      GeocodingResponse data = GeocodingResponse.fromJson(jsonDecode(response.body.replaceAll('[', '').replaceAll(']', '')));
      return data;
    } else {
      throw Exception('Failed to get geocode information for that area');
    }
  }
}
