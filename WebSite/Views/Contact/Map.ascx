<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Site.ViewModels.BaseViewModel>" %>

<div class="top-shadow"></div>
<div class="bottom-shadow"></div>

<div id="map_canvas">
    <!-- Lat/Long: 38.631489,-90.193838 -->
    <noscript>
        <img width="100%" alt="Infuz | 611 N. Tenth Street, Fourth Floor, Saint Louis, MO 63101 | Phone: (314) 584-8000 | Fax: (314) 584-8080"
            src="http://maps.googleapis.com/maps/api/staticmap?center=611+N+10th+St,+St+Louis,+Missouri+63101&zoom=15&size=640x400&scale=2&maptype=hybrid&markers=icon:http://infuz2012.local.infuz.com/Content/images/Contact/map-icon.png|38.631489,-90.193838&format=jpg&sensor=false">
    </noscript>
</div>

<script>
    (function() {
        
        window.Infuz.initMap = function() {
        
            // Create the LatLng object (Infuz office)
            var infuzLatLng = new google.maps.LatLng(38.631489, -90.193838);
            
            // Create the Map object
            var mapOptions = {
                center: infuzLatLng,
                zoom: 15,
                minZoom: 8,
                maxZoom: 19,
                disableDefaultUI: true,
                mapTypeId: google.maps.MapTypeId.HYBRID
            };
            var map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions);
            
            // Create the InfoWindow object
            var infowindow = new google.maps.InfoWindow({
                content: '<div class="infoWindow">' +
                            '<h3><img alt="Infuz" src="/Content/images/Contact/logo-sm-blck.png"></h3>' +
                            '<p>' +
                                '611 N. Tenth Street<br>' +
                                'Fourth Floor<br>' +
                                'Saint Louis, MO 63101<br>' +
                                'Phone: (314) 584-8000<br>' +
                                'Fax: (314) 584-8080<br>' +
                            '</p>' +
                            '<p>' +
                                '<a href="http://maps.google.com/maps?daddr=611+N+10th+Saint+Louis,+MO+63101&z=12" target="_blank">Get Directions</a>' +
                            '</p>' +
                        '</div>'
            });

            // Create the Marker object
            var marker = new google.maps.Marker({
                position: infuzLatLng,
                map: map,
                title: 'Infuz',
                icon: new google.maps.MarkerImage('/Content/images/Contact/map-icon.png', null, null, new google.maps.Point(13, 58))
            });
            
            // Bind event for clicking the marker
            google.maps.event.addListener(marker, 'click', function() {
                infowindow.open(map, marker);
            });

        };
        
    })();
</script>
<script src="http://maps.googleapis.com/maps/api/js?key=AIzaSyBqJvPhoy-TICc0ajEgAtwM6n8ExKoeCeI&sensor=false&callback=window.Infuz.initMap"></script>
