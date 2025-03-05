
let _paq = window._paq = window._paq || [];

/**
 * The MatomoProvider object that provides the functionality to track the user's behavior
 * @type {{apiUrl: null, siteId: null, url: null, userId: null, referrer: null, pageTitle: null, variables: null, dimensions: null, initialize: Window.MatomoProvider.initialize, triggerPageChange: Window.MatomoProvider.triggerPageChange, resetUserId: Window.MatomoProvider.resetUserId, pushCommonTrackers(): void, setProperties(*, *, *, *, *, *): void}}
 */
window.MatomoProvider = {
    apiUrl: null,
    siteId: null,
    url: null,
    userId: null,
    referrer: null,
    pageTitle: null,
    variables: null,
    dimensions: null,

    /**
     * Initializes the Matomo tracker with the given parameters
     * @param apiUrl The URL of the Matomo API
     * @param siteId The ID of the site to track
     * @param url The URL of the page to track
     * @param userId The ID of the user to track
     * @param variables The array of custom variables to track
     * @param dimensions The array of custom dimensions to track
     */
    initialize: function (apiUrl, siteId, url, userId, variables, dimensions) {
        this.setProperties(apiUrl, siteId, url, userId, variables, dimensions);
 
        const u = this.apiUrl[this.apiUrl.length - 1] === '/' ? this.apiUrl : this.apiUrl + '/';
        _paq.push(['setTrackerUrl', u + 'matomo.php']);
        _paq.push(['setSiteId', this.siteId]);

        this.pushCommonTrackers();
        
        const d = document, g = d.createElement('script'), s = d.getElementsByTagName('script')[0];
        g.type = 'text/javascript'; g.async = true; g.defer = true; g.src = u + 'matomo.js'; s.parentNode.insertBefore(g, s);

    },

    /**
     *  Triggers a page change event in the tracker
     *  @param referrerUrl The URL of the referrer page
     *  @param currentUrl The URL of the current page
     */
    triggerPageChange: function (referrerUrl, currentUrl) {
        this.url = currentUrl;
        this.referrer = referrerUrl;
        this.pageTitle = document.title;
        
        this.pushCommonTrackers();
    },

    /**
     *  Resets the user identifier in the tracker
     */
    resetUserId: function () {
        _paq.push(['resetUserId']);
        _paq.push(['appendToTrackingUrl', 'new_visit=1']);
        _paq.push(['trackPageView']);
        _paq.push(['appendToTrackingUrl', '']);
    },

    /**
     *  Pushes the common trackers to the Matomo tracker
     */
    pushCommonTrackers() {
        if (this.userId) {
            _paq.push(['setUserId', this.userId]);
        }

        if (this.variables && this.variables.length > 0) {
            this.variables.forEach(element => {
                _paq.push(['setCustomVariable', element.slot, element.name, element.value, element.scope]);
            });
        }
        
        if(this.dimensions && this.dimensions.length > 0) {
            this.dimensions.forEach(element => {
                _paq.push(['setCustomDimension', element.id, element.value]);
            });
        }
        

        if (this.url) {
            _paq.push(['setCustomUrl', this.url]);
        }
        
        if (this.referrer) {
            _paq.push(['setReferrerUrl', this.referrer]);
        }
        
        if (this.pageTitle) {
            _paq.push(['setDocumentTitle', this.pageTitle]);
        }
        
        _paq.push(['trackPageView']);
        _paq.push(['enableLinkTracking']);
    },

    /**
     *  Sets the properties of the Matomo tracker
     * @param apiUrl The URL of the Matomo API
     * @param siteId The ID of the site to track
     * @param url The URL of the page to track
     * @param userId The ID of the user to track
     * @param variables The array of custom variables to track
     * @param dimensions The array of custom dimensions to track
     */
    setProperties(apiUrl, siteId, url, userId, variables, dimensions) {
        this.apiUrl = apiUrl;
        this.siteId = siteId;
        this.url = url;
        this.userId = userId;
        this.variables = variables;
        this.dimensions = dimensions;
    }
}

