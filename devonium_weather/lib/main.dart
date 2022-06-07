import 'package:flutter/material.dart';

import 'app_loader.dart';

///Initializes the Flutter application. Ensures no issues occur on app load by ensuring the initialization of the widgets
///This weather app uses: https://www.color-hex.com/color-palette/27299 for a nice color scheme
void main() {
  WidgetsFlutterBinding.ensureInitialized();
  runApp(AppLoader());
}
