

//var dbphotolist = db.Photos.Where(x => x.ItemId == item.ItmId).ToList();
//foreach (var photo in dbphotolist)
//{
//    db.Photos.Remove(photo);
//}

//List<PhotoModel> photoM = new List<PhotoModel>();
//photoM = updateıtem.Photos.Select(x => new PhotoModel
//{
//    Id = x.Id,
//    ItemId = x.ItemId,
//    Photos = x.Photo
//}).ToList();

////photoo = item.PhotoFiles;
//foreach (var photonew in photoM)
//{
//    db.Photos.Add(new Photos()
//    {
//        Photo = photonew.Photos
//    });

//}

//mesajmodel.Mesaj = updateıtem.Item + " ürünü başarıyla güncellendi...";




//Sipariş detayını gizlemek için "Gizle" butonu oluşturup gizlemiştik. 
//function gizle(id)
//{

//    console.log("tıklandııı");  //fonksiyonun içine girip girmediğini öğrenmek için console ekranına yazdırıyoruz.

//    var elems = document.getElementsByClassName("orderdetail_" + id); //Hangi SiparişDetayGöster butonuna tıkladığını tutar.aslında 1 tane ama yinede döngüye alırız farklı durumlar olabilir.

//    console.log(elems.length);
//    for (var i = 0; i < elems.length; i += 1)
//    {
//        elems[i].style.display = 'none';

//    }

//}

//</ script >
//}




 //< div class= "form-group" >
 //        @Html.LabelFor(m => m.OrderMCustomerId)
 //       @Html.DropDownListFor(m => m.OrderMCustomerId, new SelectList(Model.CustomerVMM, "Id", "NameSurname"), "Müşteri Seçiniz", new { @class = "form-control" })
 //       @Html.ValidationMessageFor(m => m.OrderMCustomerId, "", new { @style = "color:red" })
 //   </ div >