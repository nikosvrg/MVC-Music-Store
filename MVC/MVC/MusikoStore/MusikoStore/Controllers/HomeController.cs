using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusikoStore.Models;

namespace MusikoStore.Controllers
{
    public class HomeController : Controller
    {
        private ChinookEntities db = new ChinookEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
           
            return View();
        }

        public ActionResult Contact()
        {
            
            return View();
        }

        public ActionResult Database()
        {
            return View();
        }

        public ActionResult Reports()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TopArtists(string StartDate, string EndDate, string ArtistAmount)
        {
            DateTime startDate;
            DateTime endDate;
            int artistAmount;
             

          

            if (StartDate =="")
            {
                // StartDate in db
                startDate = DateTime.Parse("2009-01-01 00:00:00.000");
            }
            else
            {
                startDate = DateTime.Parse(StartDate);
            }

            if (EndDate == "")
            {
                //end date in db
                endDate = DateTime.Parse("2013-12-22 00:00:00.000");
            }
            else
            {
                endDate = DateTime.Parse(EndDate);
            }

            if (ArtistAmount =="")
            {
                //bigint
                artistAmount = 2144583647;
            }

            else
            {
                artistAmount = Int32.Parse(ArtistAmount);

            }

            /* SQL QUERY in Linq
             
             SELECT * FROM Artist
             INNER JOIN
            (SELECT TOP 5 Artist.ArtistId, count(*) as counter
             From Artist
             INNER JOIN Album
             On Artist.ArtistID = Album.ArtistId
             INNER JOIN Track
             ON Album.AlbumId = Track.AlbumId
             INNER JOIN InvoiceLine
             ON Track.TrackId = InvoiceLine.TrackId
             INNER JOIN Invoice
             ON InvoiceLine.InvoiceId = Invoice.InvoiceId
             WHERE InvoiceDate BETWEEN '2009-01-01 00:00:00.000' AND '2013-12-31 00:00:00.000'
             GROUP BY Artist.ArtistId , Artist.Name
             ORDER BY counter DESC) as top5
			 ON top5.ArtistId = Artist.ArtistId
			;

             */

            var result = from art in db.Artists
                         join alb in db.Albums on art.ArtistId equals alb.ArtistId
                         join t in db.Tracks on alb.AlbumId equals t.AlbumId
                         join il in db.InvoiceLines on t.TrackId equals il.TrackId
                         join i in db.Invoices on il.InvoiceId equals i.InvoiceId
                         where (i.InvoiceDate >= startDate) && (i.InvoiceDate <= endDate)
                         group art by art.ArtistId into g
                         let count = g.Count()
                         orderby count descending
                         select new
                         {
                             ArtistId = g.Key,
                             Name = g.Select(m => m.Name).FirstOrDefault()

                         };



            var artists = new List<Artist>();
           
            int counter = 0;
            foreach (var art in result)
            {
                if (counter >= artistAmount) { break; }

               artists.Add(new Artist()
                {
                   ArtistId = art.ArtistId,
                     Name = art.Name

                });
                counter++;
            }

            return View(artists);

        }

        [HttpPost]
        public ActionResult TopTracks(string StartDate, string EndDate, string TracksAmount)
        {


            DateTime startDate;
            DateTime endDate;
            int tracksAmount;


            TracksAmount = "10";
            

            if (StartDate == "")
            {
                startDate = DateTime.Parse("2009-01-01 00:00:00.000");
            }
            else
            {
                startDate = DateTime.Parse(StartDate);
            }

            if (EndDate == "")
            {
                endDate = DateTime.Parse("2013-12-22 00:00:00.000");
            }
            else
            {
                endDate = DateTime.Parse(EndDate);
            }

            if (TracksAmount == "")
            {
                tracksAmount = 2144583647;
            }

            else
            {
                tracksAmount = Int32.Parse(TracksAmount);

            }


            /* SQL QUERY
             * **
            SELECT * FROM TRACK
             INNER JOIN
            (SELECT TOP 10  InvoiceLine.TrackId, count(*) as counter
             From InvoiceLine
             INNER JOIN Track
             ON InvoiceLine.TrackId = Track.TrackId
             INNER JOIN Invoice
             ON InvoiceLine.InvoiceId = Invoice.InvoiceId
             WHERE InvoiceDate BETWEEN '2009-01-01 00:00:00.000' AND '2013-12-23 00:00:00.000'
             GROUP BY InvoiceLine.TrackId, Track.Name
             ORDER BY counter DESC) as top10
			 ON top10.TrackId = Track.TrackId
			;


			 */

            
            var result = (from til in db.InvoiceLines
                          join t in db.Tracks on til.TrackId equals t.TrackId
                          join i in db.Invoices on til.InvoiceId equals i.InvoiceId
                          where (i.InvoiceDate >= startDate) && (i.InvoiceDate <= endDate) 
                          group t by t.TrackId into g
                          let count = g.Count()
                          orderby count descending
                          select new
                          {
                              TrackId = g.Key,
                                    Name = g.Select(m => m.Name).FirstOrDefault(),
                                    Album = g.Select(m =>m.Album).FirstOrDefault(),
                                    Genre = g.Select(m =>m.Genre).FirstOrDefault(),
                                    Composer = g.Select(m =>m.Composer).FirstOrDefault()     
                        });


            var tracks = new List<Track>();

            int counter = 0;
            
           

            foreach (var tr in result)
            {
                if (counter >= tracksAmount) { break; }
               
                tracks.Add(new Track()
                {
 
                    TrackId = tr.TrackId,
                    Name = tr.Name,
                    Album = tr.Album,
                    Genre = tr.Genre,
                    Composer = tr.Composer

                });
                
                counter++;

                
            }

            return View(tracks);
        }

        [HttpPost]
        public ActionResult TopGenre( string Date, string TopGenre)
        {

            DateTime startDate;
            DateTime endDate;
            int topGenre;

            TopGenre = "1";


                if (Date == "2009")
                {
                    startDate = DateTime.Parse("2009-01-01 00:00:00.000");
                    endDate = DateTime.Parse("2009-12-31 23:59:59.000");


                }

                else if (Date == "2010")
                {
                    startDate = DateTime.Parse("2010-01-01 00:00:00.000");
                    endDate = DateTime.Parse("2010-12-31 23:59:59.000");
                }

               else if (Date == "2011")
                {
                    startDate = DateTime.Parse("2011-01-01 00:00:00.000");
                    endDate = DateTime.Parse("2011-12-31 23:59:59.000");
                }

              else  if (Date == "2012")
                {
                    startDate = DateTime.Parse("2012-01-01 00:00:00.000");
                    endDate = DateTime.Parse("2012-12-31 23:59:59.000");
                }

              else  if (Date == "2013")
                {
                    startDate = DateTime.Parse("2013-01-01 00:00:00.000");
                    endDate = DateTime.Parse("2013-12-31 23:59:59.000");
                }

                else
                 {
                startDate = DateTime.Parse("");
                endDate = DateTime.Parse("");
                 }
            
                 if  (TopGenre == "")
                {
                    topGenre = 2144583647;
                }

                else
                {
                    topGenre = Int32.Parse(TopGenre);

                }

            /* SQL Query to Linq
             * 
             * 
             SELECT* FROM Genre 
                INNER JOIN
                 (SELECT TOP 1 Genre.GenreId, count(*) as counter
                 From Genre
                 INNER JOIN Track
                 ON Genre.GenreId = Track.GenreId
                 INNER JOIN InvoiceLine
                 ON Track.TrackId = InvoiceLine.TrackId
                 INNER JOIN Invoice
                 ON InvoiceLine.InvoiceId = Invoice.InvoiceId
                 WHERE InvoiceDate BETWEEN '2009-01-01 00:00:00.000' AND '2009-12-31 00:00:00.000'
                 GROUP BY Genre.GenreId, Genre.Name
                 ORDER BY counter DESC) as top5
                 ON top5.GenreId = Genre.GenreId;

             */


            var result = from g in db.Genres
                         join t in db.Tracks on g.GenreId equals t.GenreId
                         join il in db.InvoiceLines on t.TrackId equals il.TrackId
                         join i in db.Invoices on il.InvoiceId equals i.InvoiceId
                         where (i.InvoiceDate >= startDate) && (i.InvoiceDate <= endDate)
                         group g by g.GenreId into g
                         let count = g.Count()
                         orderby count descending
                         select new
                         {
                             GenreId = g.Key,
                             Genre = g.Select(m => m.Name).FirstOrDefault()

                         };



            var genres = new List<Genre>();

            int counter = 0;
            foreach (var g in result)
            {
                if (counter >= topGenre) { break; }

                genres.Add(new Genre()
                {
                    GenreId = g.GenreId,
                    Name = g.Genre

                });
                counter++;
            }





            return View(genres);
        }




    }

   


}
 