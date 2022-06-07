import 'package:devonium_weather/helper/ad_helper.dart';
import 'package:flutter/cupertino.dart';
import 'package:google_mobile_ads/google_mobile_ads.dart';

class AdCard extends StatefulWidget {
  @override
  _AdCardState createState() => _AdCardState();
}

class _AdCardState extends State<AdCard> {
  BannerAd _bannerAd;
  bool _isBannerAdReady;
  bool _bannerSetUpStarted;

  @override
  void initState() {
    _isBannerAdReady = false;
    _bannerSetUpStarted = false;
  }

  @override
  void didChangeDependencies() {
    super.didChangeDependencies();
    if (!_bannerSetUpStarted) {
      _bannerSetUpStarted = true;

      _bannerAd = BannerAd(
        adUnitId: AdHelper.bannerAdUnitId,
        request: AdRequest(),
        size: AdSize.largeBanner,
        listener: AdListener(
          onAdLoaded: (_) {
            setState(() {
              _isBannerAdReady = true;
            });
          },
          onAdFailedToLoad: (ad, err) {
            print('Failed to load a banner ad: ${err.message}');
            _isBannerAdReady = false;
            ad.dispose();
          },
        ),
      );

      _bannerAd.load();
    }
  }

  @override
  Widget build(BuildContext context) {
    return Center(
        child: (_isBannerAdReady)
            ? Align(
                alignment: Alignment.topCenter,
                child: Container(
                  width: _bannerAd.size.width.toDouble(),
                  height: _bannerAd.size.height.toDouble(),
                  child: AdWidget(ad: _bannerAd),
                ),
              )
            : SizedBox(height: 3));
  }

  @override
  void dispose() {
    _bannerAd.dispose();
    super.dispose();
  }
}
