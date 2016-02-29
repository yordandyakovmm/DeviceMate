var Grid = {
    $grid: '',
    $form: '',
    url: '',
    lastSortColumn: '',
    lastSortDirection: '',
    sortExpression: '',
    sortColumn: '',
    sortDirection: '',

    init: function ($grid, $form, url) {
        this.$grid = $grid;
        this.$form = $form;
        this.url = url;

        Grid.$grid.on('click', '.pagination a', function (e) {
            var $this = $(this);
            var hrefValue = $this.attr('href');
            var page = hrefValue.substr(hrefValue.indexOf('#') + 1);

            $('#Pager_Page').val(page);

            var formData = Grid.$form.serialize();
            Grid.ajaxPost(formData, Grid.setPriorities);

            e.preventDefault();
            return false;
        });

        Grid.$grid.on('click', '#search-table th[data-sort-column]', function (e) {
            var $this = $(this), newSortExpression;
            Grid.sortColumn = $this.attr('data-sort-column');
            if (e.ctrlKey) {
                if (Grid.sortExpression) {
                    newSortExpression = '';
                    var direction;
                    Grid.processSortExpression(
                        function () {
                            newSortExpression += ',';
                        },
                        function (sortExpressionItemDirection) {
                            direction = (sortExpressionItemDirection == 'ASC' ? 'DESC' : 'ASC');
                        },
                        function (sortExpressionItemColumn, sortExpressionItemDirection) {
                            if (direction) {
                                newSortExpression += sortExpressionItemColumn + ' ' + direction;
                                direction = null;
                            } else {
                                newSortExpression += sortExpressionItemColumn + ' ' + sortExpressionItemDirection;
                            }
                        },
                        function () {
                            newSortExpression += ',' + Grid.sortColumn + ' ' + 'ASC';
                        }
                    );

                    Grid.sortExpression = newSortExpression;
                } else {
                    Grid.sortExpression = '';
                    if (Grid.lastSortColumn && Grid.lastSortDirection) {
                        if (Grid.lastSortColumn != Grid.sortColumn) {
                            Grid.sortExpression += Grid.lastSortColumn + ' ' + (Grid.lastSortDirection > 0 ? 'ASC' : 'DESC') + ',' + Grid.sortColumn + ' ' + 'ASC';
                        } else {
                            Grid.sortExpression += Grid.sortColumn + ' ' + (Grid.lastSortDirection < 0 ? 'ASC' : 'DESC');
                        }
                    } else {
                        Grid.sortExpression += Grid.sortColumn + ' ' + 'ASC';
                    }
                }

                Grid.lastSortColumn = null;
                Grid.lastSortDirection = null;
                Grid.sortColumn = '';
                Grid.sortDirection = '';
            } else {
                if (Grid.sortExpression) {
                    Grid.processSortExpression(
                        null,
                        function (sortExpressionItemDirection) {
                            Grid.sortDirection = (sortExpressionItemDirection == 'ASC' ? -1 : 1);
                        },
                        null,
                        function () {
                            Grid.sortDirection = 1;
                        }
                    );
                } else {
                    if (!Grid.lastSortDirection || Grid.lastSortColumn != Grid.sortColumn) {
                        Grid.sortDirection = 1;
                    } else {
                        Grid.sortDirection = -Grid.lastSortDirection;
                    }
                }

                Grid.sortExpression = '';
                Grid.lastSortColumn = Grid.sortColumn;
                Grid.lastSortDirection = Grid.sortDirection;
            }

            Grid.submitForm(e, e.ctrlKey ? Grid.setPriorities : null);
        });

        Grid.$grid.on('change', '#Pager_PageSize', function (e) {
            var $this = $(this);
            $("#Pager_PageSize").val($this.val());

            return Grid.submitForm(e, Grid.setPriorities);
        });

        Grid.$form.submit(function (e) {
            Grid.sortExpression = '';
            Grid.sortColumn = '';
            Grid.sortDirection = '';
            Grid.lastSortColumn = '';
            Grid.lastSortDirection = '';

            return Grid.submitForm(e);
        });

        Grid.activateMultiselect();
        //Grid.multiselectPatchForFirefox();
    },

    setPriorities: function() {
        var
            i,
            columnsDirectionsLength,
            column,
            priority,
            columnsDirections = Grid.sortExpression.split(',')
        ;

        for (i = 0, columnsDirectionsLength = columnsDirections.length; i < columnsDirectionsLength; ++i) {
            column = columnsDirections[i].substr(0, columnsDirections[i].indexOf(' '));
            priority = i + 1;
            $('#search-table > thead > tr > th[data-sort-column="' + column + '"] > sup').text(priority);
        }
    },

    submitForm: function (e, afterSuccess) {
        var formData;

        $('#Sorter_Expression').val(Grid.sortExpression);
        $('#Sorter_Column').val(Grid.sortColumn);
        $('#Sorter_Direction').val(Grid.sortDirection);

        $('#Pager_Page').val(0);

        var hdnVisibleColumns = $("#ColumnSelector_UserGridColumnsIdsSer");
        var columnsSelection = [];

        $(':checkbox[name="multiselect_columnSelector"]:checked').each(function () {
            var $this = $(this);
            columnsSelection.push($this.val());
        });

        hdnVisibleColumns.val(columnsSelection.length ? columnsSelection.join(',') : '');
        

        formData = Grid.$form.serialize();

        Grid.ajaxPost(formData, afterSuccess);

        e.preventDefault();
        return false;
    },

    ajaxPost: function (formData, afterSuccess) {
        $.ajax({
            url: Grid.url,
            cache: false,
            type: 'POST',
            data: formData,
            success: function (data) {
                Grid.$grid.html(data);
                Grid.activateMultiselect();
                EmployeesPresenter.init();

                if (afterSuccess) {
                    afterSuccess();
                }
            }
        });
    },

    processSortExpression: function (callbackOnNonFirstIterationStart, callbackOnMatch, callbackOnIterationEnd, callbackOnNoMatch) {
        var sortExpressionItems,
            hasMatch,
            sortExpressionItemIndex,
            sortExpressionItemsLength,
            sortExpressionItem,
            sortExpressionPart,
            sortExpressionItemColumn,
            sortExpressionItemDirection
        ;

        sortExpressionItems = Grid.sortExpression.split(',');
        hasMatch = false;
        for (sortExpressionItemIndex = 0, sortExpressionItemsLength = sortExpressionItems.length;
            sortExpressionItemIndex < sortExpressionItemsLength;
            ++sortExpressionItemIndex
        ) {
            if (sortExpressionItemIndex > 0) {
                if (callbackOnNonFirstIterationStart)
                    callbackOnNonFirstIterationStart();
            }

            sortExpressionItem = $.trim(sortExpressionItems[sortExpressionItemIndex]);
            sortExpressionPart = sortExpressionItem.split(' ');
            sortExpressionItemColumn = sortExpressionPart[0];
            sortExpressionItemDirection = sortExpressionPart[1];
            if (sortExpressionItemColumn === Grid.sortColumn) {
                if (callbackOnMatch)
                    callbackOnMatch(sortExpressionItemDirection);
                hasMatch = true;
            }

            if (callbackOnIterationEnd)
                callbackOnIterationEnd(sortExpressionItemColumn, sortExpressionItemDirection);
        }

        if (!hasMatch) {
            if (callbackOnNoMatch)
                callbackOnNoMatch();
        }
    },

    activateMultiselect: function () {
        var submitForm = function (e, ui) {
            if (!$(':checkbox[name="multiselect_columnSelector"]:checked').length) {
                alert('You must select at least one column.');
                return;
            }

            Grid.submitForm(e);
        };

        $("#columnSelector").multiselect({
            selectedText: "# Columns Selected"
        });

        var $ul = $('.ui-multiselect-checkboxes');
        $ul.on('click', '#columnsSelectionOK', submitForm);
        $ul.on('click', '#columnsSelectionCancel', function () {
            $('button.ui-multiselect').click();
        });

        $ul.append(
            '<li>' +
                '<input type="button" class="btn" id="columnsSelectionOK" value="OK" />' +
                '&nbsp;' +
                '<input type="button" class="btn" id="columnsSelectionCancel" value="Cancel" />' +
            '</li>'
        );

    },

    multiselectPatchForFirefox: function() {

        var multiSelectListValues = [];
        $('#columnSelector > option[selected]').each(function () {
            var $this = $(this);
            multiSelectListValues.push($this.attr('value'));
        });

        var checkboxes = $('.ui-multiselect-checkboxes label input:checkbox');
        var numberOfSelectedColumns = 0;
        checkboxes.each(function () {
            var $this = $(this),
                found = false,
                i;

            for (i = 0; i < multiSelectListValues.length; i++) {
                if (multiSelectListValues[i] === $this.val()) {
                    $this.attr('checked', 'checked');
                    found = true;
                    numberOfSelectedColumns++;
                    break;;
                }
            }

            if (!found) {
                $this.removeAttr('checked');
            }
        });


        $('button.ui-multiselect').text(numberOfSelectedColumns + ' selected');
    }
}