/*
 * jQuery UI Multicolumn Autocomplete Widget Plugin 2.0
 * Copyright (c) 2012 Mark Harmon
 *
 * Depends:ui-widget-header"
 *   - jQuery UI Autocomplete widget
 *
 * Dual licensed under the MIT and GPL licenses:
 *   http://www.opensource.org/licenses/mit-license.php
 *   http://www.gnu.org/licenses/gpl.html
*/
$.widget('custom.mcautocomplete', $.ui.autocomplete, {
    _renderMenu: function (ul, items) {
        var self = this, thead;

        if (this.options.showHeader) {
            table = $('<div class="tr_bgcolor" style="width:100%"></div>');
            $.each(this.options.columns, function (index, item) {
                table.append('<span style="padding:0 4px;float:left;width:' + item.width + ';">' + item.name + '</span>');
            });
            table.append('<div style="clear: both;"></div>');
            ul.append(table);
        }
        $.each(items, function (index, item) {
            self._renderItem(ul, item);
        });
    },
    _renderItem: function (ul, item) {
        var t = '',
			result = '';

        $.each(this.options.columns, function (index, column) {
            if (item[column.valueField] != 'undefined') {
                t += '<span style="padding:0 4px;float:left;width:' + column.width + ';">' + item[column.valueField ? column.valueField : index] + '</span>'
            }
        });
    //  alert(item.CPTCode);
        result = $('<li></li>')
			.data('item.autocomplete', item)
			.append('<a class="mcacAnchor">' + t + '<div style="clear: both;"></div></a>')
			.appendTo(ul);
        return result;
    }
});
