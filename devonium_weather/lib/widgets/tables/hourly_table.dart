import 'package:devonium_weather/helper/app_localizations.dart';
import 'package:devonium_weather/provider/hourly_change_notifier.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

class HourlyTable extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    AppLocalizations al = AppLocalizations.of(context);
    final model = context.watch<HourlyChangeNotifier>();

    return Semantics(
      //https://stackoverflow.com/questions/60020972/flutter-what-is-the-best-way-to-designed-a-data-table-to-fit-the-full-screen-i
      child: SingleChildScrollView(
        scrollDirection: Axis.vertical,
        child: SingleChildScrollView(scrollDirection: Axis.horizontal, child: _buildWithData(al, model.hourlyInformation)),
      ),
      readOnly: true,
      label: AppLocalizations.of(context).translate('HourlyTableHelper'),
    );
  }

  //Takes a correct index from 0-8 to select the row header, then  it requires the data for each cell in the row other than the first.
  //If the rowData list is null, N/A will be inserted
  DataRow _buildDataRow(int index, List<String> rowData, AppLocalizations al) {
    String rowHeader;

    switch (index) {
      case 0:
        rowHeader = al.translate('TemperatureHeader');
        break;
      case 1:
        rowHeader = al.translate('FeelsLikeHeader');
        break;
      case 2:
        rowHeader = al.translate('PrecipitationPossibilityHeader');
        break;
      case 3:
        rowHeader = al.translate('HumidityHeader');
        break;
      case 4:
        rowHeader = al.translate('DewPointHeader');
        break;
      case 5:
        rowHeader = al.translate('CloudCoverageHeader');
        break;
      case 6:
        rowHeader = al.translate('VisibilityHeader');
        break;
      case 7:
        rowHeader = al.translate('WindSpeedHeader');
        break;
      case 8:
        rowHeader = al.translate('WindDirection');
        break;
      default:
        throw Exception('The index is not supported please use 0-8');
    }

    if (rowData == null) {
      return DataRow(cells: [DataCell(Text(rowHeader)), for (int i = 0; i < 5; i++) DataCell(Text('N/A'))]);
    }

    //https://stackoverflow.com/questions/50441168/iterating-through-a-list-to-render-multiple-widgets-in-flutter
    return DataRow(cells: [DataCell(Text(rowHeader)), for (String s in rowData) DataCell(Text(s))]);
  }

  Widget _buildWithData(AppLocalizations al, List<List<String>> data) {
    TextStyle headerStyle = TextStyle(color: Color.fromARGB(255, 201, 228, 202));

    return DataTable(
      columns: [
        DataColumn(label: Text('', style: headerStyle)),
        DataColumn(label: Text(al.translate('NowHeader'), style: headerStyle)),
        DataColumn(label: Text(al.translate('OneHourHeader'), style: headerStyle)),
        DataColumn(label: Text(al.translate('TwoHourHeader'), style: headerStyle)),
        DataColumn(label: Text(al.translate('ThreeHourHeader'), style: headerStyle)),
        DataColumn(label: Text(al.translate('FourHourHeader'), style: headerStyle)),
      ],
      rows: [
        if (data == null)
          for (int i = 0; i < 9; i++) _buildDataRow(i, null, al)
        else
          for (int i = 0; i < 9; i++) _buildDataRow(i, data.elementAt(i), al)
      ],
    );
  }
}
