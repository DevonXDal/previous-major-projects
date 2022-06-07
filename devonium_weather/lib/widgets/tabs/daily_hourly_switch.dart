import 'package:devonium_weather/helper/app_localizations.dart';
import 'package:devonium_weather/widgets/tables/daily_table.dart';
import 'package:devonium_weather/widgets/tables/hourly_table.dart';
import 'package:flutter/material.dart';

class DailyHourlySwitch extends StatelessWidget {
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
                  child: Tab(
                    text: AppLocalizations.of(context).translate('DailyHeader'),
                    icon: Icon(Icons.calendar_today),
                  ),
                  label: AppLocalizations.of(context).translate('DailyHeaderHelper'),
                  readOnly: true,
                  button: true,
                ),
                Semantics(
                  child: Tab(text: AppLocalizations.of(context).translate('HourlyHeader'), icon: Icon(Icons.access_time)),
                  label: AppLocalizations.of(context).translate('HourlyHeaderHelper'),
                  readOnly: true,
                  button: true,
                )
              ],
            ),
          ),
          Container(
              height: 320,
              child: TabBarView(children: [
                DailyTable(),
                HourlyTable(),
              ]))
        ]));
  }
}
