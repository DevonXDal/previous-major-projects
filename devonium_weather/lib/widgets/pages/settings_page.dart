import 'package:devonium_weather/data/app_state.dart';
import 'package:devonium_weather/helper/app_localizations.dart';
import 'package:devonium_weather/helper/unit_type.dart';
import 'package:flutter/material.dart';

class SettingsPage extends StatefulWidget {
  @override
  _SettingsPageState createState() => _SettingsPageState();
}

class _SettingsPageState extends State<SettingsPage> {
  bool tempMetric = AppState.settingSetForTemperature == UnitType.metric;
  bool speedMetric = AppState.settingSetForSpeed == UnitType.metric;
  bool distanceMetric = AppState.settingSetForDistance == UnitType.metric;

  @override
  Widget build(BuildContext context) {
    AppLocalizations al = AppLocalizations.of(context);
    TextStyle headerStyle = TextStyle(color: const Color.fromARGB(255, 201, 228, 202), fontWeight: FontWeight.bold, fontSize: 24);

    return SafeArea(
      child: Scaffold(
          appBar: AppBar(
            backgroundColor: Color.fromARGB(255, 43, 139, 127),
            title: Semantics(
              child: Text(al.translate('AppName')),
              label: al.translate('AppNameHelper'),
              readOnly: true,
            ),
          ),
          backgroundColor: const Color.fromARGB(255, 16, 100, 102),
          body: Column(
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            children: [
              Center(
                child: Text(
                  al.translate('Units'),
                  style: headerStyle,
                ),
              ),
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                children: [
                  Text(al.translate('Temp')),
                  Switch(
                    value: tempMetric,
                    onChanged: (value) {
                      AppState.settingSetForTemperature = (value) ? UnitType.metric : UnitType.imperial;
                      setState(() {
                        tempMetric = value;
                      });
                    },
                    activeTrackColor: Colors.lightGreenAccent,
                    activeColor: Colors.green,
                  ),
                ],
              ),
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                children: [
                  Text(al.translate('Distance')),
                  Switch(
                    value: distanceMetric,
                    onChanged: (value) {
                      AppState.settingSetForDistance = (value) ? UnitType.metric : UnitType.imperial;
                      setState(() {
                        distanceMetric = value;
                      });
                    },
                    activeTrackColor: Colors.lightGreenAccent,
                    activeColor: Colors.green,
                  ),
                ],
              ),
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                children: [
                  Text(al.translate('Speed')),
                  Switch(
                    value: speedMetric,
                    onChanged: (value) {
                      AppState.settingSetForSpeed = (value) ? UnitType.metric : UnitType.imperial;
                      setState(() {
                        speedMetric = value;
                      });
                    },
                    activeTrackColor: Colors.lightGreenAccent,
                    activeColor: Colors.green,
                  ),
                ],
              ),
            ],
          )),
    );
  }
}
