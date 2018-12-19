$(".Datefield").datepicker({
    dateFormat: 'mm/dd/yy',
    changeMonth: true,
    changeYear: true,    
    showWeeks: true,
    inline: true,
    numberOfMonths: [1, 1],
    yearRange: "2010:2020",
    showOn: "button",
    buttonImage: "../Images/calender.gif",
    buttonImageOnly: true
});