import 'package:devonium_weather/helper/weather_router.dart';
import 'package:devonium_weather/provider/daily_change_notifier.dart';
import 'package:devonium_weather/provider/hourly_change_notifier.dart';
import 'package:devonium_weather/provider/location_change_notifier.dart';
import 'package:devonium_weather/provider/lunar_table_change_notifier.dart';
import 'package:devonium_weather/provider/solar_table_change_notifier.dart';
import 'package:devonium_weather/repository/app_state_repository.dart';
import 'package:devonium_weather/repository/geocoding_repository.dart';
import 'package:devonium_weather/repository/weather_repository.dart';
import 'package:devonium_weather/widgets/pages/location_page.dart';
import 'package:devonium_weather/widgets/pages/settings_page.dart';
import 'package:flutter/material.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:provider/provider.dart';

import 'helper/app_localizations.dart';
import 'widgets/pages/weather_page.dart';

class AppLoader extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    WeatherRepository weatherRepository = WeatherRepository();
    AppStateRepository appStateRepository = AppStateRepository();

    return MaterialApp(
      localizationsDelegates: [
        AppLocalizations.delegate,
        GlobalMaterialLocalizations.delegate,
        GlobalWidgetsLocalizations.delegate,
      ],
      supportedLocales: [
        Locale('en', ''),
        Locale('es', ''),
      ],
      title: 'Devonium Weather',
      theme: ThemeData(
        primarySwatch: Colors.blue,
        textTheme: TextTheme(
          //https://stackoverflow.com/questions/49203740/how-to-change-the-entire-themes-text-color-in-flutter
          bodyText1: TextStyle(),
          bodyText2: TextStyle(),
          headline1: TextStyle(),
        ).apply(
          bodyColor: const Color.fromARGB(255, 240, 240, 240),
          displayColor: const Color.fromARGB(255, 240, 240, 240),
        ),
      ),
      initialRoute: WeatherRouter.homePage,
      routes: {
        WeatherRouter.homePage: (context) => ChangeNotifierProvider(
              child: ChangeNotifierProvider(
                child: ChangeNotifierProvider(
                  child: ChangeNotifierProvider(
                    child: ChangeNotifierProvider(
                      child: WeatherPage(),
                      create: (context) => LocationChangeNotifier(appStateRepository, GeocodingRepository()),
                    ),
                    create: (context) => DailyChangeNotifier(weatherRepository),
                  ),
                  create: (context) => HourlyChangeNotifier(weatherRepository),
                ),
                create: (context) => SolarTableChangeNotifier(weatherRepository),
              ),
              create: (context) => LunarTableChangeNotifier(weatherRepository),
            ),
        WeatherRouter.locationPage: (context) => ChangeNotifierProvider(
              child: LocationPage(),
              create: (context) => LocationChangeNotifier(appStateRepository, GeocodingRepository()),
            ),
        WeatherRouter.settingsPage: (context) => SettingsPage()
      },
    );
  }
}
