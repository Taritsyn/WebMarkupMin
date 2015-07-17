var webmarkupmin;

(function (webmarkupmin, undefined) {
	"use strict";

	webmarkupmin.registerNamespace = function (namespaceString) {
		var parts = namespaceString.split("."),
			parent = webmarkupmin,
			i;

		if (parts[0] === "webmarkupmin") {
			parts = parts.slice(1);
		}

		for (i = 0; i < parts.length; i += 1) {
			if (typeof parent[parts[i]] === "undefined") {
				parent[parts[i]] = {};
			}
			parent = parent[parts[i]];
		}

		return parent;
	};

	webmarkupmin.hasScrollbar = function(elem) {
		return (elem.clientHeight < elem.scrollHeight);
	};
}(webmarkupmin = webmarkupmin || {}));