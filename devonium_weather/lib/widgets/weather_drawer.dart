import 'package:devonium_weather/data/location_header_data.dart';
import 'package:devonium_weather/helper/app_localizations.dart';
import 'package:devonium_weather/helper/weather_router.dart';
import 'package:devonium_weather/provider/daily_change_notifier.dart';
import 'package:devonium_weather/provider/hourly_change_notifier.dart';
import 'package:devonium_weather/provider/location_change_notifier.dart';
import 'package:devonium_weather/provider/lunar_table_change_notifier.dart';
import 'package:devonium_weather/provider/solar_table_change_notifier.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

class WeatherDrawer extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    final model = context.watch<LocationChangeNotifier>();

    return _buildWithData(context, model.locationData);
  }

  Widget _buildInitial(BuildContext context) {
    return Drawer(
      child: Text('Please Wait For App To Load'),
    );
  }

  Widget _buildWithData(BuildContext context, LocationHeaderData data) {
    if (data == null || data.locationName == null || data.options == null) {
      return _buildInitial(context);
    }

    AppLocalizations al = AppLocalizations.of(context);
    TextStyle headerStyle = TextStyle(color: const Color.fromARGB(255, 201, 228, 202), fontWeight: FontWeight.bold, fontSize: 14);

    return Drawer(
      // Add a ListView to the drawer. This ensures the user can scroll
      // through the options in the drawer if there isn't enough vertical
      // space to fit everything.
      child: Container(
        decoration: BoxDecoration(color: const Color.fromARGB(255, 16, 100, 102)),
        child: ListView(
          // Important: Remove any padding from the ListView.
          padding: EdgeInsets.zero,
          children: <Widget>[
            DrawerHeader(
              child: Center(
                child: Row(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    Text('${al.translate('DrawerCurrentLocationHeader')}:\n${data.locationName}'),
                  ],
                ),
              ),
              decoration: BoxDecoration(
                color: const Color.fromARGB(255, 43, 139, 127),
              ),
            ),
            ListTile(
              title: Text(
                al.translate('RemoveHeader'),
                style: headerStyle,
              ),
            ),
            for (int i = 0; i < data.options.length; i++)
              Material(
                child: InkWell(
                  child: ListTile(
                    title: Text(data.options.elementAt(i)),
                  ),
                  splashColor: Colors.blue,
                  onLongPress: () => _removeLocation(data.options.elementAt(i), context, data),
                ),
                color: const Color.fromARGB(255, 16, 100, 102),
              ),
            SizedBox(
              height: 25,
            ),
            ListTile(
              title: Text(al.translate('SettingsHeader')),
              onTap: () => Navigator.pushNamed(context, WeatherRouter.settingsPage),
            )
          ],
        ),
      ),
    );
  }

  void _removeLocation(String newName, BuildContext context, LocationHeaderData data) async {
    if (newName == data.locationName) {
      return;
    }

    LocationChangeNotifier locationHeaderCN = context.read<LocationChangeNotifier>();

    locationHeaderCN.removeLocation(newName);

    DailyChangeNotifier dailyTableCN = context.read<DailyChangeNotifier>();
    HourlyChangeNotifier hourlyTableCN = context.read<HourlyChangeNotifier>();
    SolarTableChangeNotifier solarTableCN = context.read<SolarTableChangeNotifier>();
    LunarTableChangeNotifier lunarTableCN = context.read<LunarTableChangeNotifier>();

    dailyTableCN.updateWeatherInformation();
    hourlyTableCN.updateWeatherInformation();
    solarTableCN.updateSolarInformation();
    lunarTableCN.updateLunarInformation();
  }
}
