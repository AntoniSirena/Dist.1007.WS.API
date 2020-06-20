﻿using JS.Base.WS.API.Base;
using JS.Base.WS.API.Controllers.Generic;
using JS.Base.WS.API.DBContext;
using JS.Base.WS.API.Helpers;
using JS.Base.WS.API.Models.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static JS.Base.WS.API.Global.Constants;

namespace JS.Base.WS.API.Controllers.Domain
{
    [RoutePrefix("api/tanda")]
    [Authorize]
    public class TandaController : GenericApiController<Tanda>
    {
        private MyDBcontext db;
        private Response response;


        public TandaController()
        {
            db = new MyDBcontext();
            response = new Response();
        }

        private long currentUserId = CurrentUser.GetId();

        public override IHttpActionResult Create(dynamic entity)
        {
            string inputShortName = entity["ShortName"];
            var tandaShortName = db.Tandas.Where(x => x.ShortName == inputShortName && x.IsActive == true).FirstOrDefault();

            if (tandaShortName != null)
            {
                response.Code = InternalResponseCodeError.Code309;
                response.Message = InternalResponseCodeError.Message309;

                return Ok(response);
            }

            string inputName = entity["Name"];
            var tandaName = db.Tandas.Where(x => x.Name == inputName && x.IsActive == true).FirstOrDefault();

            if (tandaName != null)
            {
                response.Code = InternalResponseCodeError.Code310;
                response.Message = InternalResponseCodeError.Message310;

                return Ok(response);
            }

            object input = JsonConvert.DeserializeObject<object>(entity.ToString());
            return base.Create(input);
        }


        public override IHttpActionResult Update(dynamic entity)
        {

            string inputShortName = entity["ShortName"];
            int idInput = Convert.ToInt32(entity["Id"]);
            var tandaShortName = db.Tandas.Where(x => x.ShortName == inputShortName && x.IsActive == true).FirstOrDefault();

            if (tandaShortName != null)
            {
                if (idInput != tandaShortName.Id)
                {
                    response.Code = InternalResponseCodeError.Code309;
                    response.Message = InternalResponseCodeError.Message309;

                    return Ok(response);
                }
            }

            string inputName = entity["Name"];
            var tandaName = db.Tandas.Where(x => x.Name == inputName && x.IsActive == true).FirstOrDefault();

            if (tandaName != null)
            {
                if (idInput != tandaName.Id)
                {
                    response.Code = InternalResponseCodeError.Code310;
                    response.Message = InternalResponseCodeError.Message310;

                    return Ok(response);
                }
            }

            object input = JsonConvert.DeserializeObject<object>(entity.ToString());
            return base.Update(input);
        }
    }
}
