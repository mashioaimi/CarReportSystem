using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
            //dgvCarData.DataSource = _cars;
        }


        private CarReport.CarMaker MakerSelect()
        {
            if (rbToyota.Checked == true)
            {
                return CarReport.CarMaker.トヨタ;
            } else if (rbNissan.Checked == true)
            {
                return CarReport.CarMaker.日産;
            } else if (rbHonda.Checked == true)
            {
                return CarReport.CarMaker.ホンダ;
            } else if (rbGaisya.Checked == true)
            {
                return CarReport.CarMaker.外車;
            } else if (rbSonota.Checked == true)
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
            var test = dgvCarData.CurrentRow.Cells[2].Value;
            /*CarReport selectedCar = _cars[dgvCarData.CurrentRow.Index];
            cbName.Text = selectedCar.Author;
            cbCarName.Text = selectedCar.Name;
            tbReport.Text = selectedCar.Report;
            */
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            if (cbName.Text == "") //(==)→中身の確認
            {
                MessageBox.Show("正しい値を入力してください。",
                    "エラー",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            CarReport obj = new CarReport()
            {
                CreatedDate = dtCreateTime.Value,
                Author = cbName.Text,
                Name = cbCarName.Text,
                Maker = MakerSelect(),
                Report = tbReport.Text,
                Picture = pbImage.Image
            };

            setComboBoxAuthor(cbName.Text);



            _cars.Insert(0, obj); //リストの先頭(インデックス0)へ追加
            dgvCarData.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            inputItemAllClear();

            buttonClear();

            initButton();
            dgvCarData.ClearSelection(); //クリックしたときに選択されないようにする
            dgvCarData.ClearSelection(); //選択行をクリア

        }

        private void inputItemAllClear()
        {
            cbName.Text = null;
            cbCarName.Text = "";
            MakerSelect();
            tbReport.Text = "";
            pbImage.Image = null;
        }

        private void initButton()
        {
            if (_cars.Count <= 0)
            {
                btDelet.Enabled = false;
            } else
            {
                btDelet.Enabled = true;
            }
        }

        private void setComboBoxAuthor(string Name)
        {
            if (!cbName.Items.Contains(Name)) //!←否定
            {
                //コンボボックスの候補に追加
                cbName.Items.Add(Name);
            }
        }

        private void btClearImage_Click(object sender, EventArgs e)
        {
            if (pbImage.Image == null)
            {
                return;
            } else
            {
                pbImage.Image = null;
            }
        }

        private void btModify_Click(object sender, EventArgs e)
        {
            dgvCarData.CurrentRow.Cells[1].Value = dtCreateTime.Value;
            dgvCarData.CurrentRow.Cells[2].Value = cbName.Text;
            dgvCarData.CurrentRow.Cells[3].Value = MakerSelect();
            dgvCarData.CurrentRow.Cells[4].Value = cbCarName.Text;
            dgvCarData.CurrentRow.Cells[5].Value = tbReport.Text;

            if (pbImage.Image != null)
            {
                dgvCarData.CurrentRow.Cells[6].Value = ImageToByteArray(pbImage.Image);
            } else
            {
                dgvCarData.CurrentRow.Cells[6].Value = null;

            }


            //データベース更新（反映）
            this.Validate();
            this.carReportBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.infosys202008DataSet);
        }

        private void buttonClear()
        {
            rbToyota.Checked = false;
            rbNissan.Checked = false;
            rbHonda.Checked = false;
            rbSubaru.Checked = false;
            rbGaisya.Checked = false;
            rbSonota.Checked = false;
        }

        private void btDelet_Click(object sender, EventArgs e)
        {
            _cars.RemoveAt(dgvCarData.CurrentRow.Index);
            initButton();

        }

        private void btOpen_Click(object sender, EventArgs e)
        {
            //オープンファイルダイアログを表示
            /*if (ofdOpenData.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = new FileStream(ofdOpenData.FileName, FileMode.Open))
                {
                    try
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        //逆シリアル化して読み込む
                        _cars = (BindingList<CarReport>)formatter.Deserialize(fs);
                        //データグリッドビューに再設定
                        dgvCarData.DataSource = _cars;
                        //選択されている箇所を各コントロールへ表示
                        dgvCarData_Click(sender, e);

                    } catch (SerializationException se)
                    {
                        Console.WriteLine("Failed to deserialize. Reason: " + se.Message);
                        throw;
                    }
                }
            }*/
            this.carReportTableAdapter.Fill(this.infosys202008DataSet.CarReport);
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            dgvCarData.CurrentRow.Cells[2].Value = cbName.Text;

            //データベース反映
            this.Validate();
            this.carReportBindingSource.EndEdit();
            this.carReportTableAdapter.Fill(this.infosys202008DataSet.CarReport);


        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: このコード行はデータを 'infosys202008DataSet.CarReport' テーブルに読み込みます。必要に応じて移動、または削除をしてください。
            dgvCarData.Columns[0].Visible = false; //id非表示にする
            initButton();
        }

        private void carReportBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            //データベース更新（反映）
            this.Validate();
            this.carReportBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.infosys202008DataSet);

        }

        private void carReportBindingNavigatorSaveItem_Click_1(object sender, EventArgs e)
        {
            this.Validate();
            this.carReportBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.infosys202008DataSet);

        }


        private void dgvCarData_Click_1(object sender, EventArgs e)
        {
            try
            {

                dtCreateTime.Value = (DateTime)dgvCarData.CurrentRow.Cells[1].Value;
                var Maker = dgvCarData.CurrentRow.Cells[3].Value;

                cbName.Text = dgvCarData.CurrentRow.Cells[2].Value.ToString();
                cbCarName.Text = dgvCarData.CurrentRow.Cells[4].Value.ToString();
                tbReport.Text = dgvCarData.CurrentRow.Cells[5].Value.ToString();

                //ラジオボタンの設定
                setRadioButtonMaker((string)Maker);

                pbImage.Image = ByteArrayToImage((byte[])dgvCarData.CurrentRow.Cells[6].Value);

            }
            catch (InvalidCastException)
            {
                pbImage.Image = null;
            } 
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void setRadioButtonMaker(string carMaker)
        {
            switch (carMaker)
            {
                case "トヨタ":
                    rbToyota.Checked = true;
                    break;

                case "日産":
                    rbNissan.Checked = true;
                    break;

                case "ホンダ":
                    rbHonda.Checked = true;
                    break;

                case "スバル":
                    rbSubaru.Checked = true;
                    break;

                case "外車":
                    rbGaisya.Checked = true;
                    break;

                case "その他":
                    rbSonota.Checked = true;
                    break;
            }
        }

        // バイト配列をImageオブジェクトに変換
        public static Image ByteArrayToImage(byte[] byteData)
        {
            ImageConverter imgconv = new ImageConverter();
            Image img = (Image)imgconv.ConvertFrom(byteData);
            return img;
        }

        // Imageオブジェクトをバイト配列に変換
        public static byte[] ImageToByteArray(Image img)
        {
            ImageConverter imgconv = new ImageConverter();
            byte[] byteData = (byte[])imgconv.ConvertTo(img, typeof(byte[]));
            return byteData;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.carReportTableAdapter.FillByCarName(this.infosys202008DataSet.CarReport,tbSerchCarName.Text);
            this.carReportTableAdapter.FillByCrenderData(this.infosys202008DataSet.CarReport,dtpSerchCrenderData.Value.ToString());
            this.carReportTableAdapter.FillByMaker(this.infosys202008DataSet.CarReport, tbSerchMaker.Text);
        }
    }
}
