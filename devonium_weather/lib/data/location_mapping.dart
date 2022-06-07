class LocationMapping {
  final String locationName;
  final double latitude;
  final double longitude;

  const LocationMapping(this.locationName, this.latitude, this.longitude);

  LocationMapping.fromJson(Map<String, dynamic> json)
      : locationName = json['name'],
        latitude = json['lat'],
        longitude = json['long'];

  Map<String, dynamic> toJson() => {
        'name': locationName,
        'lat': latitude,
        'long': longitude,
      };
}
