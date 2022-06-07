import 'package:bezier_chart/bezier_chart.dart';
import 'package:devonium_weather/helper/app_localizations.dart';
import 'package:devonium_weather/provider/daily_change_notifier.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

class DailyPopChart extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    AppLocalizations al = AppLocalizations.of(context);
    final model = context.watch<DailyChangeNotifier>();

    return Center(child: _buildWithData(al, model.popChartInformation, context));
  }

  Widget _buildInitial() {
    return Text('N/A');
  }

  Widget _buildWithData(AppLocalizations al, List<num> data, BuildContext context) {
    if (data == null) {
      return _buildInitial();
    }

    DateTime dateToday = DateTime.now();

    return Semantics(
      child: Container(
        color: Colors.red,
        height: MediaQuery.of(context).size.height / 1.25,
        width: MediaQuery.of(context).size.width,
        child: BezierChart(
          bezierChartScale: BezierChartScale.WEEKLY,
          fromDate: dateToday,
          toDate: dateToday.add(Duration(days: 4)),
          series: [
            BezierLine(
              label: al.translate('PopLabel'),
              data: List.generate(data.length, (index) {
                return DataPoint<DateTime>(value: data.elementAt(index).toDouble(), xAxis: dateToday.add(Duration(days: index)));
              }),
            ),
          ],
          config: BezierChartConfig(
            verticalIndicatorStrokeWidth: 2.0,
            verticalIndicatorColor: Colors.black12,
            showVerticalIndicator: true,
            displayYAxis: true,
            stepsYAxis: 20,
            startYAxisFromNonZeroValue: true,
            contentWidth: MediaQuery.of(context).size.width,
            backgroundColor: Color.fromARGB(255, 43, 139, 127),
          ),
        ),
      ),
      label: al.translate('DailyPopChartHelper'),
    );
  }
}
