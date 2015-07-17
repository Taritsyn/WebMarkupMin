/// <reference path="../jquery-1.9.1.js" />

(function (webmarkupmin, $, undefined) {
	"use strict";

	var $minificationForm,
		$minificationInputField,
		$minificationInputClearButton,
		$minifyButton
		;

	$(function () {
		$minificationForm = $("form[data-form-type='minification-form']");
		$minificationInputField = $(":input[data-control-type='minification-input-field']", $minificationForm);
		$minificationInputClearButton = $("<div class=\"minification-input-clear-button\" title=\"Clear text\"></div>");
		$minifyButton = $(":input[data-control-type='minify-button']", $minificationForm);

		$minificationForm.on("submit", onMinificationFormSubmitHandler);

		$minificationInputClearButton.on("click", onMinificationInputClearButtonClickHandler);
		$minificationInputField.parent().append($minificationInputClearButton);
		refreshMinificationInputClearButton();
		$minificationInputField
			.on("input propertychange keydown keyup paste", onMinificationInputFieldChangeHandler)
			;

		$minifyButton.removeAttr("disabled");
	});

	$(window).unload(function() {
		$minificationForm.off("submit", onMinificationFormSubmitHandler);

		$minificationInputClearButton
			.off("click", onMinificationInputClearButtonClickHandler)
			.remove()
			;

		$minificationInputField
			.off("input propertychange keydown keyup paste", onMinificationInputFieldChangeHandler)
			;

		$minificationForm = null;
		$minificationInputField = null;
		$minificationInputClearButton = null;
		$minifyButton = null;
	});

	var refreshMinificationInputClearButton = function() {
		if ($.trim($minificationInputField.val()).length > 0) {
			$minificationInputClearButton.show();
		} else {
			$minificationInputClearButton.hide();
		}

		if (webmarkupmin.hasScrollbar($minificationInputField.get(0))) {
			$minificationInputClearButton.addClass("with-scrollbar");
		}
		else {
			$minificationInputClearButton.removeClass("with-scrollbar");
		}
	};

	var onMinificationFormSubmitHandler = function () {
		var $form = $(this);
		if ($form.valid()) {
			$minifyButton.attr("disabled", "disabled");
			$("textarea[data-control-type='minification-output-field']", $form).val('');

			return true;
		}

		return false;
	};

	var onMinificationInputFieldChangeHandler = function () {
		refreshMinificationInputClearButton();
	};

	var onMinificationInputClearButtonClickHandler = function() {
		$minificationInputField.val("");

		var $button = $(this);
		$button.hide();
	};
}(webmarkupmin, jQuery));