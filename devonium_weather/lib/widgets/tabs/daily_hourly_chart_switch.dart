import 'package:devonium_weather/helper/app_localizations.dart';
import 'package:devonium_weather/widgets/charts/daily_pop_chart.dart';
import 'package:devonium_weather/widgets/charts/daily_temperature_chart.dart';
import 'package:devonium_weather/widgets/charts/hourly_pop_chart.dart';
import 'package:devonium_weather/widgets/charts/hourly_temperature_chart.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';

class DailyHourlyChartSwitch extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    AppLocalizations al = AppLocalizations.of(context);
    TextStyle headerStyle = TextStyle(color: const Color.fromARGB(255, 201, 228, 202), fontWeight: FontWeight.bold, fontSize: 24);

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
                    text: al.translate('DailyHeader'),
                    icon: Icon(Icons.calendar_today),
                  ),
                  label: al.translate('DailyChartHeaderHelper'),
                  readOnly: true,
                  button: true,
                ),
                Semantics(
                  child: Tab(text: al.translate('HourlyHeader'), icon: Icon(Icons.access_time)),
                  label: al.translate('HourlyChartHeaderHelper'),
                  readOnly: true,
                  button: true,
                )
              ],
            ),
          ),
          Container(
              height: 1280,
              child: TabBarView(children: [
                Column(
                  children: [
                    Text(
                      al.translate('TemperatureHeader'),
                      style: headerStyle,
                    ),
                    DailyTemperatureChart(),
                    SizedBox(
                      height: 50,
                    ),
                    Text(
                      al.translate('PrecipitationPossibilityHeader'),
                      style: headerStyle,
                    ),
                    DailyPopChart()
                  ],
                ),
                Column(
                  children: [
                    Text(
                      al.translate('TemperatureHeader'),
                      style: headerStyle,
                    ),
                    HourlyTemperatureChart(),
                    SizedBox(
                      height: 50,
                    ),
                    Text(
                      al.translate('PrecipitationPossibilityHeader'),
                      style: headerStyle,
                    ),
                    HourlyPopChart()
                  ],
                ),
              ]))
        ]));
  }
}
