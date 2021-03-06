﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eCommerce_Csharp_Cards.Interfaces;
using eCommerce_Csharp_Cards.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace eCommerce_Csharp_Cards.Controllers {
    [Authorize]
    [Route ("Cards/[controller]")]
    public class XboxController : Controller {
        private readonly ICards _Cards;
        public XboxController (ICards Cards) => this._Cards = Cards;
        [HttpGet]
        public async Task<IActionResult> Get () {
            try {
                IEnumerable<Cards> Cards = await _Cards.GetXbox ();
                if (Cards == null) return StatusCode (StatusCodes.Status406NotAcceptable, "No Hay Documentos");
                return Ok (JsonConvert.SerializeObject (Cards));
            } catch (Exception) {
                return BadRequest ("Error vuelva a intentar");
            }
        }

        [HttpGet ("{id}")]
        public async Task<IActionResult> Get (string Id) {
            try {
                if (string.IsNullOrEmpty (Id) || Id.Length < 24) return StatusCode (StatusCodes.Status406NotAcceptable, "Id Invalid");
                Cards Card = await _Cards.GetXbox (Id);
                if (Card == null) return StatusCode (StatusCodes.Status406NotAcceptable, "No Hay Documentos");
                return Ok (JsonConvert.SerializeObject (Card));
            } catch (Exception) {
                return BadRequest ("Error vuelva a intentar");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post ([FromBody] Cards Card) {
            try {
                if (!ModelState.IsValid) return StatusCode (StatusCodes.Status406NotAcceptable, ModelState);
                await _Cards.PostXbox (Card);
                return await Colecciones();
            } catch (Exception) {
                return BadRequest ("Ha Ocurrido Un Error Vuelva A Intentar");
            }
        }

        // PUT api/values/5
        [HttpPut ("{id}")]
        public async Task<IActionResult> Put (string Id, [FromBody] Cards Card) {
            try {
                if (string.IsNullOrEmpty (Id) || Id.Length < 24) return StatusCode (StatusCodes.Status406NotAcceptable, "Id Invalid");
                if (!ModelState.IsValid) return StatusCode (StatusCodes.Status406NotAcceptable, ModelState);
                Card.Id = Id;
                var h = await _Cards.PutXbox (Id, Card);
                if (h.MatchedCount > 0) return await Colecciones();
                else return StatusCode (StatusCodes.Status406NotAcceptable, "No Editado");
            } catch (Exception) {
                return BadRequest ("Ha Ocurrido Un Error Vuelva A Intentar");
            }
        }

        [HttpDelete ("{id}")]
        public async Task<IActionResult> Delete (string Id) {
            try {
                if (string.IsNullOrEmpty (Id) || Id.Length < 24) return StatusCode (StatusCodes.Status406NotAcceptable, "Id Invalid");
                var h = await _Cards.DeleteXbox (Id);
                if (h.DeletedCount > 0) return await Colecciones();
                else return StatusCode (StatusCodes.Status406NotAcceptable, "No Eliminado");
            } catch (Exception) {
                return BadRequest ("Ha Ocurrido Un Error Vuelva A Intentar");
            }
        }
        public async Task<IActionResult> Colecciones () {
            List<IEnumerable<Cards>> Cards = new List<IEnumerable<Cards>> ();
            Cards.Add (await _Cards.GetAmazon ());
            Cards.Add (await _Cards.GetGooglePlay ());
            Cards.Add (await _Cards.GetiTunes ());
            Cards.Add (await _Cards.GetPaypal ());
            Cards.Add (await _Cards.GetPlayStation ());
            Cards.Add (await _Cards.GetSteam ());
            Cards.Add (await _Cards.GetXbox ());
            return Ok (JsonConvert.SerializeObject (Cards));
        }
    }
}