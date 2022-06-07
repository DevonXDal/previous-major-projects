import 'package:devonium_weather/helper/app_localizations.dart';
import 'package:devonium_weather/widgets/tables/lunar_table.dart';
import 'package:devonium_weather/widgets/tables/solar_table.dart';
import 'package:flutter/material.dart';

class DayNightSwitch extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return DefaultTabController(
        length: 2, // length of tabs
        child: Column(children: [
          Container(
            child: TabBar(
              labelColor: Colors.yellow,
              unselectedLabelColor: const Color.fromARGB(255, 240, 240, 240),
              tabs: [
                Semantics(
                  child: Tab(text: AppLocalizations.of(context).translate('SunlightHeader'), icon: Icon(Icons.wb_sunny)),
                  label: AppLocalizations.of(context).translate('SunlightHeaderHelper'),
                  readOnly: true,
                  button: true,
                ),
                Semantics(
                  child: Tab(text: AppLocalizations.of(context).translate('MoonlightHeader'), icon: Icon(Icons.nightlight_round)),
                  label: AppLocalizations.of(context).translate('MoonlightHeaderHelper'),
                  readOnly: true,
                  button: true,
                ),
              ],
            ),
          ),
          Container(height: 320, child: TabBarView(children: [Center(child: SolarTable()), Center(child: LunarTable())]))
        ]));
  }
}
