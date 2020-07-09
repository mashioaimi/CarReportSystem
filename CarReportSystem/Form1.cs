using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarReportSystem
{
    public partial class Form1 : Form
    {
        //車データを入れるバインディングリスト
        BindingList<CarReport> _cars = new BindingList<CarReport>();

        public Form1()
        {
            InitializeComponent();
            dgvCarData.DataSource = _cars;
        }


        private CarReport.CarMaker MakerSelect()
        {
            if (rbToyota.Checked == true)
            {
                return CarReport.CarMaker.トヨタ;
            }
            else if(rbNissan.Checked == true)
            {
                return CarReport.CarMaker.日産;
            }
            else if(rbHonda.Checked == true)
            {
                return CarReport.CarMaker.ホンダ;
            }
            else if(rbGaisya.Checked == true)
            {
                return CarReport.CarMaker.外車;
            }
            else if(rbSonota.Checked == true)
            {
                return CarReport.CarMaker.その他;
            }
            return MakerSelect();

        }


        private void btOpenImage_Click(object sender, EventArgs e)
        {
            if (ofdOpenImage.ShowDialog() == DialogResult.OK)
            {
                //選択した画像をピクチャーボックスに表示
                pbImage.Image = Image.FromFile(ofdOpenImage.FileName);
                //ピクチャーボックスのサイズに画像を調整
                pbImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }

        }

        private void dgvCarData_Click(object sender, EventArgs e)
        {
            CarReport selectedCar = _cars[dgvCarData.CurrentRow.Index];
            cbName.Text = selectedCar.Author;
            cbCarName.Text = selectedCar.Name;
            tbReport.Text = selectedCar.Report;
        }

        private void btAdd_Click_1(object sender, EventArgs e)
        {
            CarReport obj = new CarReport()
            {
                CreatedDate = dtCreateTime.Value,
                Author = cbName.Text,
                Name = cbCarName.Text,
                Maker = MakerSelect(),
                Report = tbReport.Text,
                Picture = pbImage.Image
            };
            _cars.Insert(0, obj);

        }

        private void btClearImage_Click(object sender, EventArgs e)
        {
            pbImage.Image = null; //クリックされたらbtImageを何も入っていない状態にする
        }

        private void btModify_Click(object sender, EventArgs e)
        {
            //変更対象のレコード(オブジェクト)
            CarReport modify = new CarReport() //中の処理を変更
            {
                CreatedDate = dtCreateTime.Value,
                Author = cbName.Text,
                Name = cbCarName.Text,
                Maker = MakerSelect(),
                Report = tbReport.Text,
                Picture = pbImage.Image
            };

            _cars[dgvCarData.CurrentRow.Index] = modify; //格納された行の文字を変更

        }

        private void btDelet_Click(object sender, EventArgs e)
        {
            _cars.RemoveAt(dgvCarData.CurrentRow.Index);

        }
    }
}
