using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StandardCan.jwt;
using StandardCan.Models;

namespace StandardCan.Service
{
    public class book_room_configService
    {


        public messageModel update(book_room_configModel value)
        {
            messageModel result = new messageModel();

            try
            {
                using (var context = new StandardCanEntities())
                {
                    if (String.IsNullOrEmpty(value.user_id))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    int ret = context.sp_cancelRoom_update(value.id, value.timeconfig);
                }

                result.status = "S";
                result.message = "";
            }
            catch (Exception ex)
            {
                result.status = "E";
                result.message = ex.Message.ToString();
            }

            return result;
        }

        public book_room_configDetailModel detail(book_room_configModel value)
        {
            book_room_configDetailModel book_Room = new book_room_configDetailModel();
            book_Room.result = new messageModel();

            try
            {
                using (var context = new StandardCanEntities())
                {
                    if (String.IsNullOrEmpty(value.user_id))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var userId = JwtHelper.GetUserIdFromToken(value.user_id);
                    if (String.IsNullOrEmpty(userId))
                    {
                        throw new Exception("Unauthorized Access");
                    }
                    var detail = context.CNF_ROOM.SingleOrDefault();
                    if(detail != null)
                    {
                        book_Room.id = detail.CBR_ID.ToString();
                        book_Room.value = detail.CBR_AUTO_CANCEL.ToString();
                    }
                }

                book_Room.result.status = "S";
                book_Room.result.message = "";
            }
            catch (Exception ex)
            {
                book_Room.result.status = "E";
                book_Room.result.message = ex.Message.ToString();
            }

            return book_Room;
        }
    }
}