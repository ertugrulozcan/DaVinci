$(document).ready(function() {

    /*
     * ----------------------------------------------------------------------------------------
     *  PRELOADER JS
     * ----------------------------------------------------------------------------------------
     */
    $('.preloader').fadeOut();
    $('.preloader-area').delay(450).fadeOut('slow');

    /*
     * ----------------------------------------------------------------------------------------
     *  SLIDER JS
     * ----------------------------------------------------------------------------------------
     */

    if ($("#rev_slider_70_1").revolution === undefined) {
        revslider_showDoubleJqueryError("#rev_slider_70_1");
    } else {
        $('#homeSection').height($(window).height());
        $("#rev_slider_70_1").show().revolution({
            sliderType: "standard",
            jsFileLocation: "../libs/assets/libs/revolution/js/",
            sliderLayout: "fullscreen",
            stopLoop: 'off',
            stopAfterLoops: -1, //-1
            stopAtSlide: 1,
            responsiveLevels: [1200, 1024, 768, 480],
            gridwidth: [1200, 1050, 768, 480],
            gridheight: [800, 700, 1050, 500],
            delay: 100000,
            shuffle: "off",
            navigation: {
                keyboardNavigation: "on",
                onHoverStop: "on",
                arrows: {
                    style: "",
                    enable: true,
                    rtl: false,
                    hide_onmobile: false,
                    hide_onleave: false,
                    hide_delay: 200,
                    hide_delay_mobile: 1200,
                    hide_under: 0,
                    hide_over: 9999,
                    tmp: '',
                    left: {
                        container: "slider",
                        h_align: "center",
                        v_align: "bottom",
                        h_offset: -25,
                        v_offset: 20
                    },
                    right: {
                        container: "slider",
                        h_align: "center",
                        v_align: "bottom",
                        h_offset: 25,
                        v_offset: 20
                    }
                }
            },
            lazyType: "none",
            parallax: {
                type: "mouse+scroll",
                origo: "slidercenter",
                speed: 2000,
                levels: [1, 2, 3, 20, 25, 30, 35, 40, 45, 50],
                disable_onmobile: "on"
            },
            shadow: 0,
            spinner: "spinner2",
            autoHeight: "on",
            disableProgressBar: "off",
            hideThumbsOnMobile: "off",
            hideSliderAtLimit: 0,
            hideCaptionAtLimit: 0,
            hideAllCaptionAtLilmit: 0,
            debugMode: false,
        });
    }

    /*
     * ----------------------------------------------------------------------------------------
     *  CHANGE MENU BACKGROUND AND TO UP JS
     * ----------------------------------------------------------------------------------------
     */
    $(window).scroll(function() {
        var header = $('.main-header');
        if ($(this).scrollTop() > 100) {
            header.addClass('main-header-top');
            $('#backToTop').fadeIn();
        } else {
            header.removeClass('main-header-top');
            $('#backToTop').fadeOut();
        }
    });

    $('#backToTop').on("click", function() {
        $("html, body").animate({
            scrollTop: 0
        }, 800);
        return false;
    });

    /*
     * ----------------------------------------------------------------------------------------
     *  SMOTH SCROOL JS
     * ----------------------------------------------------------------------------------------
     */
    $('nav a.page-scroll').on("click", function(e) {
        var anchor = $(this);
        $('html, body').stop().animate({
            scrollTop: $(anchor.attr('href')).offset().top - 66
        }, 1000);
        e.preventDefault();
    });
    $('#header__icon').on("click", function(e) {
        var header_icon = $(this);
        $('body').toggleClass('with--sidebar');
        if ($(this).hasClass('header__icon__close')) {
            header_icon.removeClass('header__icon__close');
        } else {
            header_icon.addClass('header__icon__close');
        }
        e.preventDefault();
    });
    /*
     * ----------------------------------------------------------------------------------------
     *  USER & SCREENSHOTS JS
     * ----------------------------------------------------------------------------------------
     */
    // carousel
    $("#owl-carousel-client").owlCarousel({
        loop: true,
        margin: 35,
        nav: true,
        dots: false,
        navText: ["&#xf053", "&#xf054"],
        pag: false,
        responsive: {
            320: {
                items: 1
            },
            600: {
                items: 2
            },
            768: {
                items: 3
            },
            1024: {
                items: 3
            },
            1366: {
                items: 5
            },
            1440: {
                items: 5
            }
        }
    });
    // carousel
    $("#owl-carousel-comment").owlCarousel({
        loop: true,
        margin: 35,
        nav: true,
        dots: false,
        navText: ["&#xf053", "&#xf054"],
        pag: false,
        responsive: {
            320: {
                items: 1
            },
            600: {
                items: 1
            },
            768: {
                items: 1
            },
            1024: {
                items: 1
            },
            1366: {
                items: 1
            },
            1440: {
                items: 1
            }
        }
    });

    $("#owl-carousel-team").owlCarousel({
        loop: true,
        margin: 20,
        nav: true,
        dots: false,
        navText: ["&#xf053", "&#xf054"],
        pag: false,
        responsive: {
            320: {
                items: 1
            },
            600: {
                items: 1
            },
            768: {
                items: 2
            },
            1024: {
                items: 2
            },
            1366: {
                items: 3
            },
            1440: {
                items: 3
            }
        }
    });

    $("#owl-carousel-portfolio").owlCarousel({
        loop: true,
        margin: 20,
        nav: true,
        dots: false,
        navText: ["&#xf053", "&#xf054"],
        pag: false,
        responsive: {
            320: {
                items: 1
            },
            600: {
                items: 1
            },
            768: {
                items: 1
            },
            1024: {
                items: 1
            },
            1366: {
                items: 1
            },
            1440: {
                items: 1
            }
        }
    });

    /*
     * ----------------------------------------------------------------------------------------
     *  ISOTOPE JS
     * ----------------------------------------------------------------------------------------
     */
    // activate isotope in container
    var $grid = $('#portfolio_items').isotope({
        itemSelector: '.grid-item',
        layoutMode: 'fitRows'
    });
    /*
      $('#portfolio_items').isotope({
        layoutMode: 'masonry',
        itemSelector: '.grid-item',
        percentPosition: true,
        masonry: {
          columnWidth: '.grid-sizer'
        }
      });*/
    // add isotope click function
    $('#portfolio_filter li').on("click", function() {
        var selector = $(this).attr('data-filter');
        $('#portfolio_items').isotope({
            filter: selector,
            animationOptions: {
                duration: 750,
                easing: 'linear',
                queue: false
            }
        });

        // change active class on li
        $('#portfolio_filter ul li').each(function() {
            var $portfolio_filter = $('#portfolio_filter ul li');
            $portfolio_filter.removeClass('active');
            $portfolio_filter.on('click', function() {
                $(this).addClass('active');
            });
        });
        return false;
    });

});
