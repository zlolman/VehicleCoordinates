using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.MapProviders;
using VehicleCoordinates.Repositories;
using VehicleCoordinates.Models;
using VehicleCoordinates.Interfaces;
using System.Windows;
using System.Windows.Input;

namespace VehicleCoordinates
{
    public partial class MainForm : Form
    {
        IRepository repository;
        GMapOverlay markers;
        GMapOverlay dragdropOverlay;
        int  dragdropMarkerId;
        private int deletingMarkerId;
        private bool markerDragged = false;

        public MainForm(IRepository _repository)
        {
            repository = _repository;
            InitializeComponent();
            ToolStripMenuItem aboutItem = new ToolStripMenuItem("О программе");
            aboutItem.Click += aboutItem_Click;
            menuStrip1.Items.Add(aboutItem);
            markers = new GMapOverlay("vehicles");
            dragdropOverlay = new GMapOverlay("dragdrop");
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            gMapControl.DragButton = MouseButtons.Right;
            gMapControl.ShowCenter = false;
            gMapControl.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            gMapControl.Position = new GMap.NET.PointLatLng(55.751999, 37.617734);
            RefreshMarkers();
        }

        private async void RefreshMarkers() 
        {
            markers.Markers.Clear();
            gMapControl.Overlays.Clear();
            GMapMarker marker;
            List<Coordinate> coordinates = await repository.GetAll();
            foreach (Coordinate coor in coordinates)
            {
                marker = new GMarkerGoogle(new PointLatLng(coor.X_axis, coor.Y_axis), GMarkerGoogleType.red_dot);
                marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                marker.ToolTipText = "Id = " + coor.Id.ToString();
                markers.Markers.Add(marker);
            };
            gMapControl.Overlays.Add(markers);
        }

        
        //create new marker
        private void gMapControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Left))
            {
                Coordinate coordinate = new Coordinate() { X_axis = gMapControl.FromLocalToLatLng(e.X, e.Y).Lat, Y_axis = gMapControl.FromLocalToLatLng(e.X, e.Y).Lng };
                repository.Create(coordinate);
                RefreshMarkers();              
            }
        }
        //delete marker
        private void gMapControl_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right))
            {
                deletingMarkerId = Convert.ToInt32(item.ToolTipText.Substring(5));
                repository.Delete(deletingMarkerId);
                markers.Markers.Remove(item);
                gMapControl.Overlays.Clear();
                gMapControl.Overlays.Add(markers);
            }
        }

        private void gMapControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) 
            {
                if (markers.Markers.Any<GMapMarker>(mark => mark.IsMouseOver)) 
                {
                    GMapMarker marker = markers.Markers.FirstOrDefault<GMapMarker>(mark => mark.IsMouseOver);
                    markers.Markers.Remove(marker);
                    dragdropOverlay.Markers.Add(marker);
                    gMapControl.Overlays.Clear();
                    gMapControl.Overlays.Add(markers);
                    gMapControl.Overlays.Add(dragdropOverlay);
                    markerDragged = true;
                    dragdropMarkerId = Convert.ToInt32( marker.ToolTipText.Substring(5));
                }
            }
        }

        private void gMapControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (markerDragged)
            {
                gMapControl.Overlays[1].Markers.Clear();
                GMapMarker newMarker = new GMarkerGoogle(new PointLatLng(gMapControl.FromLocalToLatLng(e.X, e.Y).Lat, gMapControl.FromLocalToLatLng(e.X, e.Y).Lng), GMarkerGoogleType.red_dot);
                newMarker.ToolTipText = "Id = " + dragdropMarkerId.ToString();
                gMapControl.Overlays[1].Markers.Add(newMarker);
            }
        }

        private void gMapControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (markerDragged) 
            {
                markerDragged = false;
                GMapMarker newMarker = gMapControl.Overlays[1].Markers[0];
                gMapControl.Overlays.Clear();
                markers.Markers.Add(newMarker);
                Coordinate coordinate = new Coordinate() { Id = dragdropMarkerId, X_axis = newMarker.Position.Lat, Y_axis = newMarker.Position.Lng };
                gMapControl.Overlays.Add(markers);
                repository.Update(coordinate);
            }            
        }
        void aboutItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Двойной щелчок ЛКМ - поставить маркер\nПКМ над меркером - удалить маркер\nУдерживать ЛКМ над маркером - переместить маркер\nУдерживать ПКМ над картой - переместить карту");
        }
    }
}
