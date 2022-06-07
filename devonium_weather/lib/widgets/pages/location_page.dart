import 'package:country_state_city_picker/country_state_city_picker.dart';
import 'package:devonium_weather/helper/app_localizations.dart';
import 'package:devonium_weather/provider/location_change_notifier.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

class LocationPage extends StatefulWidget {
  @override
  _LocationPageState createState() => _LocationPageState();
}

class _LocationPageState extends State<LocationPage> {
  String countryValue;
  String stateValue;
  String cityValue;

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
              Semantics(
                child: Text(
                  al.translate('AddLocationHeader'),
                  style: headerStyle,
                ),
                readOnly: true,
                label: al.translate('AddLocationHeaderHelper'),
              ),
              SelectState(
                dropdownColor: const Color.fromARGB(255, 148, 132, 155),
                style: TextStyle(color: const Color.fromARGB(255, 240, 240, 240)),
                onCountryChanged: (value) {
                  setState(() {
                    countryValue = value;
                  });
                },
                onStateChanged: (value) {
                  setState(() {
                    stateValue = value;
                  });
                },
                onCityChanged: (value) {
                  setState(() {
                    cityValue = value;
                  });
                },
              ),
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                children: [
                  Semantics(
                    child: TextButton(
                        onPressed: () {
                          Navigator.pop(context);
                        },
                        child: Text(al.translate('BackButton'))),
                    readOnly: true,
                    button: true,
                    label: al.translate('BackButtonHelper'),
                  ),
                  Semantics(
                    child: ElevatedButton(
                        onPressed: () async {
                          if (countryValue != null && countryValue.isNotEmpty) {
                            //LocationChangeNotifier locationHeaderCN = context.read<LocationChangeNotifier>();
                            stateValue ??= '';
                            cityValue ??= '';
                            await Provider.of<LocationChangeNotifier>(context, listen: false).addLocation(cityValue, stateValue, countryValue);
                            Navigator.pop(context);
                          } else {
                            _missingDataDialog();
                          }
                        },
                        child: Text(al.translate('SubmitButton'))),
                    readOnly: true,
                    button: true,
                    label: al.translate('SubmitButtonHelper'),
                  ),
                ],
              )
            ],
          )),
    );
  }

  Future<void> _missingDataDialog() {
    return showDialog<void>(
        context: context,
        builder: (BuildContext context) {
          AppLocalizations al = AppLocalizations.of(context);

          return Semantics(
            child: AlertDialog(
              title: Text(al.translate('LocationAddFailedHeader')),
              content: SingleChildScrollView(
                child: ListBody(
                  children: <Widget>[
                    Text(al.translate('LocationAddFailedText')),
                  ],
                ),
              ),
              actions: <Widget>[
                TextButton(
                  child: Text(al.translate('OkButton')),
                  onPressed: () {
                    Navigator.of(context).pop();
                  },
                ),
              ],
            ),
            label: al.translate('LocationAddFailedHelper'),
          );
        });
  }
}
