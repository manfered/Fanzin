

$(function () {
    $(window).on("load resize", function () {
        $(".fill-screen").css("height", window.innerHeight);
    });

    //add bootstrap's scrollspy
    $('body').scrollspy({
        target: '.navbar',
        offset: 0
    });

    // smooth scrolling
    $('nav a, .welcome-screen-child a').bind('click', function () {
        $('html, body').stop().animate({
            scrollTop: $($(this).attr('href')).offset().top
        }, 1500, 'easeInOutExpo');
        event.preventDefault();
    });

    //nanogallery
    $(document).ready(function () {
        $("#nanoGallery3").nanoGallery({
            //itemsBaseURL: 'http://brisbois.fr/nanogallery/demonstration/'
            thumbnailWidth: 'auto',
            thumbnailHeight: 300,

            colorScheme: 'none',
            thumbnailHoverEffect: [{ name: 'labelAppear75', duration: 300 }],
            thumbnailGutterWidth: 0,
            thumbnailGutterHeight: 0,
            i18n: { thumbnailImageDescription: 'نمایش تصویر'},
            thumbnailLabel: { display: true, position: 'overImageOnMiddle', align: 'center' }

        });
    });

});


//contact collaps toggle script
document.querySelector('#contactus-toggle-button').addEventListener('click', function () {
    document.querySelector('.contactus-form-section.collapsible').classList.toggle('collapsed');
});