/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, {
/******/ 				configurable: false,
/******/ 				enumerable: true,
/******/ 				get: getter
/******/ 			});
/******/ 		}
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "/";
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = 109);
/******/ })
/************************************************************************/
/******/ ({

/***/ 109:
/***/ (function(module, exports, __webpack_require__) {

module.exports = __webpack_require__(110);


/***/ }),

/***/ 110:
/***/ (function(module, exports) {


(function ($) {

	"use strict";

	// RESIZABLE IMAGES

	if (isExists('.responsive-img-bg')) {

		$('.responsive-img-bg .p-item').each(function () {

			var $this = $(this),
			    imgSrc = $this.find('img').attr('src');

			$this.css({ 'background-image': 'url(' + imgSrc + ')' });
		});
	}

	// ISOTOPE

	$(window).on('load', function () {

		// ISOTOPE PORTFOLIO WITH FILTER
		if (isExists('.p-grid-isotope')) {
			$('.p-grid-isotope').isotope({
				// set itemSelector so .grid-sizer is not used in layout
				itemSelector: '.p-item',
				percentPosition: true,
				masonry: {
					columnWidth: '.grid-sizer'

				}
			});
		}
	});

	// DROPDOWN MENU

	var winWidth = $(window).width();
	dropdownMenu(winWidth);

	$(window).on('resize', function () {
		winWidth = $(window).width();
		dropdownMenu(winWidth);
	});

	// REVOLUTION SLIDER

	if (isExists('#rev_slider_28_1')) {
		var tpj = jQuery;
		var revapi28;

		if (tpj("#rev_slider_28_1").revolution == undefined) {
			revslider_showDoubleJqueryError("#rev_slider_28_1");
		} else {
			revapi28 = tpj("#rev_slider_28_1").show().revolution({
				sliderType: "standard",
				jsFileLocation: "revolution/js/",
				sliderLayout: "fullscreen",
				dottedOverlay: "none",
				delay: 4000,

				navigation: {

					keyboardNavigation: "off",
					keyboard_direction: "horizontal",
					mouseScrollNavigation: "off",
					mouseScrollReverse: "default",
					onHoverStop: "off",
					arrows: {
						style: "uranus",
						enable: true,
						hide_onmobile: false,
						hide_onleave: false,
						tmp: '',
						left: {
							h_align: "left",
							v_align: "center",
							h_offset: 20,
							v_offset: 0
						},
						right: {
							h_align: "right",
							v_align: "center",
							h_offset: 20,
							v_offset: 0
						}
					},

					bullets: {
						enable: true,
						hide_onmobile: false,
						style: "hermes",
						hide_onleave: false,
						direction: "horizontal",
						h_align: "center",
						v_align: "bottom",
						h_offset: 0,
						v_offset: 20,
						space: 5,
						tmp: ''
					}
				},
				responsiveLevels: [1240, 1024, 778, 480],
				visibilityLevels: [1240, 1024, 778, 480],
				gridwidth: [1240, 1024, 778, 480],
				gridheight: [868, 768, 960, 720],
				lazyType: "none",

				shadow: 0,
				spinner: "off",
				stopLoop: "off",
				stopAfterLoops: -1,
				stopAtSlide: -1,

				shuffle: "off",
				autoHeight: "off",
				fullScreenAutoWidth: "off",
				fullScreenAlignForce: "off",
				fullScreenOffsetContainer: "",
				fullScreenOffset: "60px",
				hideThumbsOnMobile: "off",
				hideSliderAtLimit: 0,
				hideCaptionAtLimit: 0,
				hideAllCaptionAtLilmit: 0,
				debugMode: false,
				fallbacks: {
					simplifyAll: "off",
					nextSlideOnWindowFocus: "off",
					disableFocusListener: false
				}
			});
		}
	}

	$('[data-menu]').on('click', function () {

		var mainMenu = $(this).data('menu');

		$(mainMenu).toggleClass('visible-menu');
	});

	if (isExists('.venobox')) {
		$('.venobox').attr('data-gall', 'gallery').venobox({
			framewidth: '50%', // default: ''
			frameheight: '', // default: ''
			border: '0px', // default: '0'
			bgcolor: '#5dff5e', // default: '#fff'
			titleattr: 'data-title', // default: 'title'
			numeratio: false, // default: false
			infinigall: true // default: false
		});
	}
})(jQuery);

function dropdownMenu(winWidth) {

	if (winWidth > 767) {

		$('.main-menu li.drop-down').on('mouseover', function () {
			var $this = $(this),
			    menuAnchor = $this.children('a');

			menuAnchor.addClass('mouseover');
		}).on('mouseleave', function () {
			var $this = $(this),
			    menuAnchor = $this.children('a');

			menuAnchor.removeClass('mouseover');
		});
	} else {

		$('.main-menu li.drop-down > a').on('click', function () {

			if ($(this).attr('href') == '#') return false;
			if ($(this).hasClass('mouseover')) {
				$(this).removeClass('mouseover');
			} else {
				$(this).addClass('mouseover');
			}
			return false;
		});
	}
}

function isExists(elem) {
	if ($(elem).length > 0) {
		return true;
	}
	return false;
}

/***/ })

/******/ });