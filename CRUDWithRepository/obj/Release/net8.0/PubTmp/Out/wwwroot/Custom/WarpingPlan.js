$(document).ready(function () {  
    $('select').select2(); 

   $(document).on('click', '.removeRow', function () {
    if ($('#warpMachineGrid tbody tr').length > 1) {
        
        $(this).closest('tr').remove();

        
        $('#warpMachineGrid tbody tr').each(function (index) {
            
            $(this).find('input, select, textarea').each(function () {
                const name = $(this).attr('name');
                if (name) {
                    const updatedName = name.replace(/\[\d+\]/, '[' + index + ']');  
                    $(this).attr('name', updatedName);
                }
            });
        });

       
        if ($('#warpMachineGrid tbody tr').length === 1) {
            $('#warpMachineGrid tbody tr:first .removeRow').hide();
        }
    }
});

    $(document).on('input', 'input[required], select[required]', function () {
        if ($(this).val() !== '') {
            $(this).removeClass('is-invalid');
        }
    }); 
    if ($('#warpMachineGrid tbody tr').length === 1) {
        $('#warpMachineGrid tbody tr:first .removeRow').hide();
    }
     
    $(document).on('change', '.planned-item-code', function () {
        debugger
        var description = $(this).find(':selected').data('description');
        var row = $(this).closest('tr');
        row.find('.item-description').val(description);
    });
    
      $("form.needs-validation").validate({
          errorClass: "text-danger",
          errorPlacement: function (error, element) {
              if (element.hasClass("select2-hidden-accessible")) {
                  error.insertAfter(element.next('.select2-container')); // Place error message correctly
              } else {
                  error.insertAfter(element); // Default placement
              }
          },
          rules: {
              "WarpingPlanDetails[0].MachineNo": {
                  required: true
              },
              "WarpingPlanDetails[0].PreviousProductId": {
                  required: true
              },
              "WarpingPlanDetails[0].PlannedProductId": {
                  required: true
              },
              //"WarpingPlanDetails[0].WarpingMachineId": {
              //    required: true
              //},
              "WarpingPlanDetails[0].PlannedQty": {
                  required: true
              }
          }
          ,
          messages: {
              "WarpingPlanDetails[0].MachineNo": "Please select a machine number.",
              "WarpingPlanDetails[0].PreviousProductId": "Please select a previous item.",
              "WarpingPlanDetails[0].PlannedProductId": "Please select a planned item.",
             /* "WarpingPlanDetails[0].WarpingMachineId": "Please select a warping machine.",*/
              "WarpingPlanDetails[0].PlannedQty": "Please enter planned qty (In-kgs)."
          }
      });

   })

 
