class SolarLunarInformation {
  Meta meta;
  Location location;

  SolarLunarInformation({this.meta, this.location});

  SolarLunarInformation.fromJson(Map<String, dynamic> json) {
    meta = json['meta'] != null ? new Meta.fromJson(json['meta']) : null;
    location = json['location'] != null ? new Location.fromJson(json['location']) : null;
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    if (this.meta != null) {
      data['meta'] = this.meta.toJson();
    }
    if (this.location != null) {
      data['location'] = this.location.toJson();
    }
    return data;
  }
}

class Meta {
  String licenseurl;

  Meta({this.licenseurl});

  Meta.fromJson(Map<String, dynamic> json) {
    licenseurl = json['licenseurl'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['licenseurl'] = this.licenseurl;
    return data;
  }
}

class Location {
  String height;
  String latitude;
  String longitude;
  List<Time> time;

  Location({this.height, this.latitude, this.longitude, this.time});

  Location.fromJson(Map<String, dynamic> json) {
    height = json['height'];
    latitude = json['latitude'];
    longitude = json['longitude'];
    if (json['time'] != null) {
      time = new List<Time>();
      json['time'].forEach((v) {
        time.add(new Time.fromJson(v));
      });
    }
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['height'] = this.height;
    data['latitude'] = this.latitude;
    data['longitude'] = this.longitude;
    if (this.time != null) {
      data['time'] = this.time.map((v) => v.toJson()).toList();
    }
    return data;
  }
}

class Time {
  Moonrise moonrise;
  Moonshadow moonshadow;
  Moonrise sunrise;
  Moonposition moonposition;
  Moonrise moonset;
  LowMoon lowMoon;
  LowMoon solarnoon;
  Moonphase moonphase;
  String date;
  LowMoon solarmidnight;
  Moonrise sunset;
  LowMoon highMoon;

  Time({this.moonrise, this.moonshadow, this.sunrise, this.moonposition, this.moonset, this.lowMoon, this.solarnoon, this.moonphase, this.date, this.solarmidnight, this.sunset, this.highMoon});

  Time.fromJson(Map<String, dynamic> json) {
    moonrise = json['moonrise'] != null ? new Moonrise.fromJson(json['moonrise']) : null;
    moonshadow = json['moonshadow'] != null ? new Moonshadow.fromJson(json['moonshadow']) : null;
    sunrise = json['sunrise'] != null ? new Moonrise.fromJson(json['sunrise']) : null;
    moonposition = json['moonposition'] != null ? new Moonposition.fromJson(json['moonposition']) : null;
    moonset = json['moonset'] != null ? new Moonrise.fromJson(json['moonset']) : null;
    lowMoon = json['low_moon'] != null ? new LowMoon.fromJson(json['low_moon']) : null;
    solarnoon = json['solarnoon'] != null ? new LowMoon.fromJson(json['solarnoon']) : null;
    moonphase = json['moonphase'] != null ? new Moonphase.fromJson(json['moonphase']) : null;
    date = json['date'];
    solarmidnight = json['solarmidnight'] != null ? new LowMoon.fromJson(json['solarmidnight']) : null;
    sunset = json['sunset'] != null ? new Moonrise.fromJson(json['sunset']) : null;
    highMoon = json['high_moon'] != null ? new LowMoon.fromJson(json['high_moon']) : null;
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    if (this.moonrise != null) {
      data['moonrise'] = this.moonrise.toJson();
    }
    if (this.moonshadow != null) {
      data['moonshadow'] = this.moonshadow.toJson();
    }
    if (this.sunrise != null) {
      data['sunrise'] = this.sunrise.toJson();
    }
    if (this.moonposition != null) {
      data['moonposition'] = this.moonposition.toJson();
    }
    if (this.moonset != null) {
      data['moonset'] = this.moonset.toJson();
    }
    if (this.lowMoon != null) {
      data['low_moon'] = this.lowMoon.toJson();
    }
    if (this.solarnoon != null) {
      data['solarnoon'] = this.solarnoon.toJson();
    }
    if (this.moonphase != null) {
      data['moonphase'] = this.moonphase.toJson();
    }
    data['date'] = this.date;
    if (this.solarmidnight != null) {
      data['solarmidnight'] = this.solarmidnight.toJson();
    }
    if (this.sunset != null) {
      data['sunset'] = this.sunset.toJson();
    }
    if (this.highMoon != null) {
      data['high_moon'] = this.highMoon.toJson();
    }
    return data;
  }
}

class Moonrise {
  String time;
  String desc;

  Moonrise({this.time, this.desc});

  Moonrise.fromJson(Map<String, dynamic> json) {
    time = json['time'];
    desc = json['desc'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['time'] = this.time;
    data['desc'] = this.desc;
    return data;
  }
}

class Moonshadow {
  String azimuth;
  String desc;
  String elevation;
  String time;

  Moonshadow({this.azimuth, this.desc, this.elevation, this.time});

  Moonshadow.fromJson(Map<String, dynamic> json) {
    azimuth = json['azimuth'];
    desc = json['desc'];
    elevation = json['elevation'];
    time = json['time'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['azimuth'] = this.azimuth;
    data['desc'] = this.desc;
    data['elevation'] = this.elevation;
    data['time'] = this.time;
    return data;
  }
}

class Moonposition {
  String phase;
  String elevation;
  String time;
  String azimuth;
  String range;
  String desc;

  Moonposition({this.phase, this.elevation, this.time, this.azimuth, this.range, this.desc});

  Moonposition.fromJson(Map<String, dynamic> json) {
    phase = json['phase'];
    elevation = json['elevation'];
    time = json['time'];
    azimuth = json['azimuth'];
    range = json['range'];
    desc = json['desc'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['phase'] = this.phase;
    data['elevation'] = this.elevation;
    data['time'] = this.time;
    data['azimuth'] = this.azimuth;
    data['range'] = this.range;
    data['desc'] = this.desc;
    return data;
  }
}

class LowMoon {
  String desc;
  String elevation;
  String time;

  LowMoon({this.desc, this.elevation, this.time});

  LowMoon.fromJson(Map<String, dynamic> json) {
    desc = json['desc'];
    elevation = json['elevation'];
    time = json['time'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['desc'] = this.desc;
    data['elevation'] = this.elevation;
    data['time'] = this.time;
    return data;
  }
}

class Moonphase {
  String desc;
  String value;
  String time;

  Moonphase({this.desc, this.value, this.time});

  Moonphase.fromJson(Map<String, dynamic> json) {
    desc = json['desc'];
    value = json['value'];
    time = json['time'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['desc'] = this.desc;
    data['value'] = this.value;
    data['time'] = this.time;
    return data;
  }
}
