import 'package:devonium_weather/helper/app_localizations.dart';
import 'package:devonium_weather/helper/weather_router.dart';
import 'package:devonium_weather/provider/daily_change_notifier.dart';
import 'package:devonium_weather/provider/hourly_change_notifier.dart';
import 'package:devonium_weather/provider/location_change_notifier.dart';
import 'package:devonium_weather/provider/lunar_table_change_notifier.dart';
import 'package:devonium_weather/provider/solar_table_change_notifier.dart';
import 'package:devonium_weather/widgets/ad_card.dart';
import 'package:devonium_weather/widgets/location_header.dart';
import 'package:devonium_weather/widgets/tabs/daily_hourly_chart_switch.dart';
import 'package:devonium_weather/widgets/tabs/day_night_switch.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import 'file:///B:/Spring2021/Mobile/LabsAndAssignments/CS321/finalproject/devonium_weather/lib/widgets/tabs/daily_hourly_switch.dart';

import '../weather_drawer.dart';

class WeatherPage extends StatefulWidget {
  WeatherPage({Key key}) : super(key: key);

  @override
  _WeatherPageState createState() => _WeatherPageState();
}

class _WeatherPageState extends State<WeatherPage> {
  @override
  void initState() {
    super.initState();
    //https://stackoverflow.com/questions/49466556/flutter-run-method-on-widget-build-complete
    WidgetsBinding.instance.addPostFrameCallback((_) {
      _refreshWeatherInformation(context);
    });
  }

  @override
  Widget build(BuildContext context) {
    AppLocalizations al = AppLocalizations.of(context);

    return SafeArea(
        child: Scaffold(
      appBar: AppBar(
        backgroundColor: const Color.fromARGB(255, 43, 139, 127),
        title: Semantics(
          child: Text(al.translate('AppName')),
          label: al.translate('AppNameHelper'),
          readOnly: true,
        ),
        actions: [
          Semantics(
              child: GestureDetector(
                  child: Icon(
                    Icons.refresh,
                    size: 30,
                  ),
                  onTap: () => _refreshWeatherInformation(context)))
        ],
      ),
      drawer: WeatherDrawer(),
      backgroundColor: const Color.fromARGB(255, 16, 100, 102),
      body: Center(
        child: SingleChildScrollView(
          scrollDirection: Axis.vertical,
          child: Column(
            children: [
              AdCard(),
              LocationHeader(),
              DailyHourlySwitch(),
              SizedBox(
                height: 75,
              ),
              DayNightSwitch(),
              SizedBox(
                height: 75,
              ),
              DailyHourlyChartSwitch()
            ],
          ),
        ),
      ),
      floatingActionButton: FloatingActionButton(child: Icon(Icons.add), onPressed: () => Navigator.pushNamed(context, WeatherRouter.locationPage)),
    ));
  }

  void _refreshWeatherInformation(BuildContext context) {
    LocationChangeNotifier locationCN = context.read<LocationChangeNotifier>();
    DailyChangeNotifier dailyTableCN = context.read<DailyChangeNotifier>();
    HourlyChangeNotifier hourlyTableCN = context.read<HourlyChangeNotifier>();
    SolarTableChangeNotifier solarTableCN = context.read<SolarTableChangeNotifier>();
    LunarTableChangeNotifier lunarTableCN = context.read<LunarTableChangeNotifier>();

    locationCN.refresh();
    dailyTableCN.updateWeatherInformation();
    hourlyTableCN.updateWeatherInformation();
    solarTableCN.updateSolarInformation();
    lunarTableCN.updateLunarInformation();
  }
}
