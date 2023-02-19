using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace WebShopping.Controllers
{
    public class HomeController : Controller
    {
        string connDB = WebConfigurationManager.ConnectionStrings["connDB"].ConnectionString;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult ProductEntry()
        {
            return View();
        }
        public ActionResult ListAllProducts()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult UserEntry()
        {
            return View();
        }

        public ActionResult UpdateProduct()
        {
            return View();
        }
        public ActionResult Orders()
        {
            return View();
        }
        public ActionResult Cart()
        {
            return View();
        }

        public ActionResult ListOfUser()
        {
            return View();
        }

        /*Add items */
        [HttpPost]
        public ActionResult ProductEntry(FormCollection collection,HttpPostedFileBase uploadImg)
        {
            var itmcde = Convert.ToString(collection["txtCode"]);
            var itmname = Request["txtname"];
            var itmdesc = Request["txtDesc"];
            var itmprce = Request["txtprice"];
            var itmonhand = Request["txtonhand"];
            var date = Request["datepicker"];

            if (uploadImg != null)
            {
                try
                {
                    string filename = Path.GetFileName(uploadImg.FileName);
                    var checkextension = Path.GetExtension(uploadImg.FileName).ToLower();
                    int filesize = uploadImg.ContentLength;
                    string logPath = "C:\\Uploads";
                    string filePath = Path.Combine(logPath, filename);
                    uploadImg.SaveAs(filePath);

                    using (var db = new SqlConnection(connDB))
                    {
                        db.Open();
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "INSERT INTO ITMTBL (ITMNUM,ITMNAME,ITMDESC,ITMPRCE,QTYHAND,ITMIMG)"
                                            + " VALUES ("
                                            + " @NUM,"
                                            + " @NAME,"
                                            + " @DESC,"
                                            + " @PRCE,"
                                            + " @QTY,"
                                            + " @IMG)";
                            cmd.Parameters.AddWithValue("@NUM", itmcde);
                            cmd.Parameters.AddWithValue("@NAME", itmname);
                            cmd.Parameters.AddWithValue("@DESC", itmdesc);
                            cmd.Parameters.AddWithValue("@PRCE", itmprce);
                            cmd.Parameters.AddWithValue("@QTY", itmonhand);
                            cmd.Parameters.AddWithValue("@IMG", filename);

                            var ctr = cmd.ExecuteNonQuery();
                            if (ctr > 0)
                                Response.Write("<script>alert('Product is Save')</script>");
                            else
                                Response.Write("<script>alert('Product is not Save')</script>");

                        }

                    }
                    
                }
                catch (Exception ex)
                {

                }
            } 
            return View();
        }
        /*provide auto itemcode upon addiing new items */
        public ActionResult getItemCode()
        {
            var data = new List<object>();
            var itmcode = "";
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using(var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT COALESCE(MAX(Id), 0) + 1 as 'MAXID' FROM ITMTBL";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        data.Add(new
                        {
                            itmcode = reader["MAXID"].ToString()
                        });
                    }
                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserLogin()
        {
            var data = new List<object>();
            var usr = Request["username"];
            var pwd = Request["pwd"];
            using(var db = new SqlConnection(connDB))
            {
                db.Open();
                using(var cmd = db.CreateCommand())
                {
                   cmd.CommandType = CommandType.Text;
                   cmd.CommandText = "SELECT * FROM USERS WHERE USERNAME='" +usr+ "'AND PASSWORD='" +pwd+ "'";
                   SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        data.Add(new
                        {
                            mess = 0
                        });
                    }
                    else
                    {
                        data.Add(new 
                        {
                            mess = 1
                        });
                    }
                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public FileResult Image (string filename)
        {
            var folder = "";
            var filepath = "";
            try
            {
                folder = "C:\\Uploads";
                filepath = Path.Combine(folder, filename);
                if (!System.IO.File.Exists(filepath))
                {
                    //image not found
                }
            }
            catch (Exception){
                //throw
            }
            var mime = System.Web.MimeMapping.GetMimeMapping(Path.GetFileName(filepath));
            Response.Headers.Add("Content-Disposition", "Inline");
            return new FilePathResult(filepath, mime);

        }


        public ActionResult SearchItem()
        {
            var data = new List<object>();
            var itmcode = Request["itemcode"];
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM ITMTBL WHERE ITMNUM='" + itmcode + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        data.Add(new
                        {
                            mess = 0,
                            desc = reader["itmdesc"].ToString(),
                            price = reader["itmprce"].ToString(),
                            qtyonhand = reader["qtyhand"].ToString(),
                        });
                    }
                    else
                    {
                        data.Add(new
                        {
                            mess = 1,
                        });
                    }
                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShoppingCart()
        {
            var data = new List<object>();
            var itmcode = Request["ITEMNO"].Trim();
            var itmname = Request["ITMNAME"].Trim();
            var price = Request["ITMPRICE"].Trim();
            var qty = Request["QTY"];
            var total = Request["TOTAL"].Trim();
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO SCART (ITMNUM,ITMNAME,ITMPRCE,QTY,TOTAL)"
                                    + " VALUES ("
                                    + " @ITEMNO,"
                                    + " @INAME,"
                                    + " @IPRICE,"
                                    + " @QUANTITY,"
                                    + " @ITMTOTAL)";
                    cmd.Parameters.AddWithValue("@ITEMNO", itmcode);
                    cmd.Parameters.AddWithValue("@INAME", itmname);
                    cmd.Parameters.AddWithValue("@IPRICE", price);
                    cmd.Parameters.AddWithValue("@QUANTITY", qty);
                    cmd.Parameters.AddWithValue("@ITMTOTAL", total);
                    var ctr = cmd.ExecuteNonQuery();
                    if (ctr > 0)
                    {
                        data.Add(new
                        {
                            mess = 0
                        });
                    }
                    else
                    {
                        data.Add(new
                        {
                            mess = 1
                        });
                    }

                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateItem()
        {
            var data = new List<object>();

            var itmcode = Request["itemcode"];
            var itmdesc = Request["itemdesc"];
            var itmprice = Request["itemprice"];
            var itmqtyonhand = Request["itemqtyonhand"];


            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE ITMTBL SET "
                        + " ITMDESC ='" + itmdesc + "',"
                        + " ITMPRCE ='" + itmprice + "',"
                        + " QTYHAND ='" + itmqtyonhand + "'"
                        + " WHERE ITMNUM='" + itmcode + "'";
                    var ctr = cmd.ExecuteNonQuery();
                    if (ctr > 0)
                    {
                        data.Add(new
                        {
                            mess = 0
                        });
                    }
                    else
                    {
                        data.Add(new
                        {
                            mess = 1
                        });
                    }
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UserEntry(FormCollection collection)
        {
            var usrid = Convert.ToString(collection["txtusrid"]);
            var usrlname = Request["txtlname"];
            var usrfname = Request["txtfname"];
            var usradd = Request["txtaddress"];
            var usrcontact = Request["txtcontact"];
            var usremail = Request["txtemail"];
            var usrpswrd = Request["txtpswrd"];
            var usrname = Request["txtusername"];
            var usrrole = Request["txtrole"];

            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO USERS (USRID,LASTNAME,FIRSTNAME,ADDRESS,CONTACTNO,EMAIL,PASSWORD,USERNAME,ROLE)"
                                    + " VALUES ("
                                    + " @UID,"
                                    + " @ULNAME,"
                                    + " @UFNAME,"
                                    + " @UADD,"
                                    + " @UCONTACT,"
                                    + " @UEMAIL,"
                                    + " @UPASS,"
                                    + " @USRNAME,"
                                    + " @ROLE)";
                    cmd.Parameters.AddWithValue("@UID", usrid);
                    cmd.Parameters.AddWithValue("@ULNAME", usrlname);
                    cmd.Parameters.AddWithValue("@UFNAME", usrfname);
                    cmd.Parameters.AddWithValue("@UADD", usradd);
                    cmd.Parameters.AddWithValue("@UCONTACT", usrcontact);
                    cmd.Parameters.AddWithValue("@UEMAIL", usremail);
                    cmd.Parameters.AddWithValue("@UPASS", usrpswrd);
                    cmd.Parameters.AddWithValue("@USRNAME", usrname);
                    cmd.Parameters.AddWithValue("@ROLE", usrrole);
                    var ctr = cmd.ExecuteNonQuery();
                    if (ctr > 0)
                        Response.Write("<script>alert('Congrats! User Successfully Registered')</script>");
                    else
                        Response.Write("<script>alert('Opss.. Something went wrong')</script>");

                }

            }

            return View();
        }
        /*Get auto userCode in create another user */
        public ActionResult getUserCode()
        {
            var data = new List<object>();
            var usrcode = "";
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT COALESCE(MAX(Id), 0) + 1 as 'MAXID' FROM USERS";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        data.Add(new
                        {
                            usrcode = reader["MAXID"].ToString()
                        });
                    }
                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RemoveItemCart()
        {
            var data = new List<object>();
            var itmcode = Request["ITEMNO"].Trim();
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "DELETE FROM SCART"
                        + " WHERE ITMNUM='" + itmcode + "'";
                    var ctr = cmd.ExecuteNonQuery();
                    if (ctr > 0)
                    {
                        data.Add(new
                        {
                            mess = 0
                        });
                    }
                    else
                    {
                        data.Add(new
                        {
                            mess = 1
                        });
                    }
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckOutOrder()
        {
            var data = new List<object>();
            var itmcode = Request["ITEMNO"].Trim();
            var itmname = Request["ITMNAME"].Trim();
            var price = Request["ITMPRICE"].Trim();
            var qty = Request["QTY"];
            var total = Request["TOTAL"].Trim();
            
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO CHECKORDERS (ITMNUM,ITMNAME,ITMPRCE,QTY,TOTAL)"
                                   + " VALUES ("
                                   + " @ITMNUM,"
                                   + " @ITMNAME,"
                                   + " @ITMPRCE,"
                                   + " @QTY,"
                                   + " @TOTAL)";
                    cmd.Parameters.AddWithValue("@ITMNUM", itmcode);
                    cmd.Parameters.AddWithValue("@ITMNAME", itmname);
                    cmd.Parameters.AddWithValue("@ITMPRCE", price);
                    cmd.Parameters.AddWithValue("@QTY", Convert.ToInt32(qty));
                    cmd.Parameters.AddWithValue("@TOTAL", total);
                    var ctr = cmd.ExecuteNonQuery();

                    if (ctr > 0)
                    {
                        data.Add(new
                        {
                            mess = 0
                        });
                    }
                    else
                    {
                        data.Add(new
                        {
                            mess = 1
                        });
                    }

                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /*Updates number of qtyonhand stocks */
        public ActionResult UpdateQtyhand()
        {
            var data = new List<object>();
            var itmcode = Request["ITEMNO"].Trim();
            var qty = Request["QTY"];
            int qtyonhand = 0;
            
            using (var db = new SqlConnection(connDB))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT QTYHAND FROM ITMTBL WHERE ITMNUM ='" + itmcode + "'";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        qtyonhand = Convert.ToInt32(reader["qtyhand"]);
                    }
                    reader.Close();
                    int newonhand = qtyonhand - Convert.ToInt32(qty);
                    cmd.CommandText = "UPDATE ITMTBL SET "
                        + " QTYHAND ='" + newonhand + "'"
                        + " WHERE ITMNUM='" + itmcode + "'";
                    var ctr = cmd.ExecuteNonQuery();
                    if (ctr > 0)
                    {
                        data.Add(new
                        {
                            mess = 0
                        });
                    }
                    else
                    {
                        data.Add(new 
                        {
                            mess = 1
                        });
                    }

                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}