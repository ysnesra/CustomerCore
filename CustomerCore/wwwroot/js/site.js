/*site.js içine yazmamızın sebebi her yerde kullanabilmek için
Departman silerken "emin misiniz" sorusunu sordurmak için */

function CheckDateTypeIsValid(dateElement) {     //fonksiyon oluşturulmuş
    var value = $(dateElement).val();
    if (value == '') {
        $(dateElement).valid("false")
    }
    else {
        $(dateElement).valid();
    }
}

//Customer Silme butonu için Onaylama Emin misin penceresi
$(function () {
    $("#tblCustomer").on("click", ".tblCustomDelete", function () {       // "tblCustomer" idli tablodaki ,".tblCustomDelete" classında click eventi olduğunda
        var btn = $(this);                    //btn değişkeninde bu butonu saklayalım.Hangi butona tıklandıysa datalarından id özelliğini getir

        bootbox.confirm("Müşteriyi silmek istediğinize emin misiniz?", function (result) { //alert yerine confirm ile evet hayırı sorguluyoruz."bootbox.confirm" alert deki gibi pencere açılmasını sağlar.
            if (result) {
                var id = btn.data("id");

                $.ajax({
                    type: "GET",
                    url: "/Customer/Delete/" + id,
                    success: function () {
                        btn.parent().parent().remove();                   //iki kez parentını alıyoruz.
                    }
                });
            }
        });
    });
});
//Category Silme butonu için Onaylama Emin misin penceresi
$(function () {
    $("#tblCategory").on("click", ".tblCategoryDelete", function () {       // "tblCustomer" idli tablodaki ,".tblCustomDelete" classında click eventi olduğunda
        var btn = $(this);                    //btn değişkeninde bu butonu saklayalım.Hangi butona tıklandıysa datalarından id özelliğini getir

        bootbox.confirm("Kategoriyi silmek istediğinize emin misiniz?", function (result) { //alert yerine confirm ile evet hayırı sorguluyoruz."bootbox.confirm" alert deki gibi pencere açılmasını sağlar.
            if (result) {
                var id = btn.data("id");

                $.ajax({
                    type: "GET",
                    url: "/Category/Delete/" + id,
                    success: function () {
                        btn.parent().parent().remove();                   //iki kez parentını alıyoruz.
                    }
                });
            }
        });
    });
});
//Ürün Silme butonu için Onaylama Emin misin penceresi
$(function () {
    $("#tblItem").on("click", ".tblItemDelete", function () {       // "tblCustomer" idli tablodaki ,".tblCustomDelete" classında click eventi olduğunda
        var btn = $(this);                    //btn değişkeninde bu butonu saklayalım.Hangi butona tıklandıysa datalarından id özelliğini getir

        bootbox.confirm("Ürünü silmek istediğinize emin misiniz?", function (result) { //alert yerine confirm ile evet hayırı sorguluyoruz."bootbox.confirm" alert deki gibi pencere açılmasını sağlar.
            if (result) {
                var id = btn.data("id");

                $.ajax({
                    type: "GET",
                    url: "/Item/Delete/" + id,
                    success: function () {
                        btn.parent().parent().remove();                   //iki kez parentını alıyoruz.
                    }
                });
            }
        });
    });
});
//Sipariş Silme butonu için Onaylama Emin misin penceresi
$(function () {
    $("#tblOrder").on("click", ".tblOrderDelete", function () {       // "tblCustomer" idli tablodaki ,".tblCustomDelete" classında click eventi olduğunda
        var btn = $(this);                    //btn değişkeninde bu butonu saklayalım.Hangi butona tıklandıysa datalarından id özelliğini getir

        bootbox.confirm("Siparişi silmek istediğinize emin misiniz?", function (result) { //alert yerine confirm ile evet hayırı sorguluyoruz."bootbox.confirm" alert deki gibi pencere açılmasını sağlar.
            if (result) {
                var id = btn.data("id");

                $.ajax({
                    type: "GET",
                    url: "/Order/Delete/" + id,
                    success: function () {
                        btn.parent().parent().remove();                   //iki kez parentını alıyoruz.
                    }
                });
            }
        });
    });
});
//Sipariş Detayı Silme butonu için Onaylama Emin misin penceresi
$(function () {
    $("#tblOrderDetails").on("click", ".tblOrderDetailsDelete", function () {       // "tblCustomer" idli tablodaki ,".tblCustomDelete" classında click eventi olduğunda
        var btn = $(this);                    //btn değişkeninde bu butonu saklayalım.Hangi butona tıklandıysa datalarından id özelliğini getir

        bootbox.confirm("Sipariş Detayını silmek istediğinize emin misiniz?", function (result) { //alert yerine confirm ile evet hayırı sorguluyoruz."bootbox.confirm" alert deki gibi pencere açılmasını sağlar.
            if (result) {
                var id = btn.data("id");

                $.ajax({
                    type: "GET",
                    url: "/OrderDetail/Delete/" + id,
                    success: function () {
                        btn.parent().parent().remove();                   //iki kez parentını alıyoruz.
                    }
                });
            }
        });
    });
});
//Adres Silme butonu için Onaylama Emin misin penceresi
$(function () {
    $("#tblAddress").on("click", ".tblAddressDelete", function () {       // "tblCustomer" idli tablodaki ,".tblCustomDelete" classında click eventi olduğunda
        var btn = $(this);                    //btn değişkeninde bu butonu saklayalım.Hangi butona tıklandıysa datalarından id özelliğini getir

        bootbox.confirm("Adresi silmek istediğinize emin misiniz?", function (result) { //alert yerine confirm ile evet hayırı sorguluyoruz."bootbox.confirm" alert deki gibi pencere açılmasını sağlar.
            if (result) {
                var id = btn.data("id");

                $.ajax({
                    type: "GET",
                    url: "/Address/Delete/" + id,
                    success: function () {
                        btn.parent().parent().remove();                   //iki kez parentını alıyoruz.
                    }
                });
            }
        });
    });
});


