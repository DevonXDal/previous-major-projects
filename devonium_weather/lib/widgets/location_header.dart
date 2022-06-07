import 'package:devonium_weather/data/location_header_data.dart';
import 'package:devonium_weather/provider/daily_change_notifier.dart';
import 'package:devonium_weather/provider/hourly_change_notifier.dart';
import 'package:devonium_weather/provider/location_change_notifier.dart';
import 'package:devonium_weather/provider/lunar_table_change_notifier.dart';
import 'package:devonium_weather/provider/solar_table_change_notifier.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../helper/app_localizations.dart';

class LocationHeader extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    final model = context.watch<LocationChangeNotifier>();

    return _buildWithData(context, model.locationData);
  }

  Widget _buildInitial(BuildContext context) {
    return _addSemantics(
        context,
        Text(
          'N/A',
          style: TextStyle(fontSize: 22),
        ));
  }

  Widget _buildLoading(BuildContext context) {
    return _addSemantics(
        context,
        Center(
          child: CircularProgressIndicator(),
        ));
  }

  Widget _buildWithData(BuildContext context, LocationHeaderData data) {
    if (data == null || data.locationName == null) {
      return _buildInitial(context);
    }

    return _addSemantics(
        context,
        DropdownButton(
          value: data.locationName,
          dropdownColor: const Color.fromARGB(255, 148, 132, 155),
          style: TextStyle(color: const Color.fromARGB(255, 240, 240, 240)),
          items: data.options.map<DropdownMenuItem<String>>((String value) {
            return DropdownMenuItem<String>(
              value: value,
              child: SizedBox(
                width: 200,
                child: Text(
                  value,
                  style: TextStyle(fontSize: 22),
                  overflow: TextOverflow.ellipsis,
                ),
              ),
            );
          }).toList(),
          onChanged: (String newValue) {
            _switchLocation(newValue, context);
          },
        ));
  }

  Widget _addSemantics(BuildContext context, Widget child) {
    return Semantics(
      child: child,
      readOnly: true,
      button: true,
      label: AppLocalizations.of(context).translate('LocationHeaderHelper'),
    );
  }

  void _switchLocation(String newName, BuildContext context) async {
    LocationChangeNotifier locationHeaderCN = context.read<LocationChangeNotifier>();

    locationHeaderCN.setWeatherFor(newName);

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
