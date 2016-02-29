var LoadingHelper = {

  /**
   * Shows a loading indicator in the button.
   *
   * @link https://github.com/hakimel/Ladda
   *
   * @param  {DOM Element} btn The DOM element to disable and show an indicator
   * @return {ladda}           New ladda instance
   */
  showIcon: function(btn) {
    var laddaInstance = $(btn).ladda();

    laddaInstance.ladda('start');

    return laddaInstance;
  },

  /**
   * Hides the loading indicator from the provided instance.
   *
   * @param  {ladda} btn The ladda instance to disable
   * @return {ladda}     The passed ladda instance (to be chained if you wish)
   */
  hideIcon: function(laddaInstance) {
    laddaInstance.ladda('stop');

    return laddaInstance;
  },

  /**
   * Hides all loading indicators.
   */
  hideAll: function() {
    $.ladda('stopAll');
  }
};
