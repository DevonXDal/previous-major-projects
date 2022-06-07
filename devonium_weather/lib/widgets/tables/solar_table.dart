import 'package:devonium_weather/helper/app_localizations.dart';
import 'package:devonium_weather/provider/solar_table_change_notifier.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

class SolarTable extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    AppLocalizations al = AppLocalizations.of(context);
    final model = context.watch<SolarTableChangeNotifier>();

    return Semantics(
      //https://stackoverflow.com/questions/60020972/flutter-what-is-the-best-way-to-designed-a-data-table-to-fit-the-full-screen-i
      child: SingleChildScrollView(
        scrollDirection: Axis.vertical,
        child: SingleChildScrollView(scrollDirection: Axis.horizontal, child: _buildWithData(al, model.solarData)),
      ),
      readOnly: true,
      label: AppLocalizations.of(context).translate('SolarTableHelper'),
    );
  }

  //Takes a correct index from 0-5 to select the row header, then  it requires the data for each cell in the row other than the first.
  //If the rowData list is null, N/A will be inserted
  DataRow _buildDataRow(int index, String rowData, AppLocalizations al) {
    String rowHeader;

    switch (index) {
      case 0:
        rowHeader = al.translate('SunriseHeader');
        break;
      case 1:
        rowHeader = al.translate('SunsetHeader');
        break;
      case 2:
        rowHeader = al.translate('SolarNoonHeader');
        break;
      case 3:
        rowHeader = al.translate('SolarMidnightHeader');
        break;
      case 4:
        rowHeader = al.translate('HoursDaylightHeader');
        break;
      case 5:
        rowHeader = al.translate('HoursDarkHeader');
        break;
      default:
        throw Exception('The index is not supported please use 0-5');
    }

    if (rowData == null) {
      return DataRow(cells: [DataCell(Text(rowHeader)), DataCell(Text('N/A'))]);
    }

    //https://stackoverflow.com/questions/50441168/iterating-through-a-list-to-render-multiple-widgets-in-flutter
    return DataRow(cells: [DataCell(Text(rowHeader)), DataCell(Text(rowData))]);
  }

  Widget _buildWithData(AppLocalizations al, List<String> data) {
    return DataTable(
      columns: [
        DataColumn(label: Text('')),
        DataColumn(label: Text('')),
      ],
      rows: [
        if (data == null)
          for (int i = 0; i < 6; i++) _buildDataRow(i, null, al)
        else
          for (int i = 0; i < 6; i++) _buildDataRow(i, data.elementAt(i), al)
      ],
    );
  }
}
