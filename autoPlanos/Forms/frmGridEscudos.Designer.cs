namespace autoPlanos.Forms
{
    partial class frmGridEscudos
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbActualiza = new System.Windows.Forms.Button();
            this.cmbCancelar = new System.Windows.Forms.Button();
            this.txtMunicipio = new System.Windows.Forms.TextBox();
            this.picFoto = new System.Windows.Forms.PictureBox();
            this.gridCatEscudo = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.picFoto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCatEscudo)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Municipio";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(207, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Imagen";
            // 
            // cmbActualiza
            // 
            this.cmbActualiza.Location = new System.Drawing.Point(5, 51);
            this.cmbActualiza.Name = "cmbActualiza";
            this.cmbActualiza.Size = new System.Drawing.Size(85, 27);
            this.cmbActualiza.TabIndex = 2;
            this.cmbActualiza.Text = "Actualiza";
            this.cmbActualiza.UseVisualStyleBackColor = true;
            this.cmbActualiza.Visible = false;
            this.cmbActualiza.Click += new System.EventHandler(this.cmbActualiza_Click);
            // 
            // cmbCancelar
            // 
            this.cmbCancelar.Location = new System.Drawing.Point(174, 50);
            this.cmbCancelar.Name = "cmbCancelar";
            this.cmbCancelar.Size = new System.Drawing.Size(75, 28);
            this.cmbCancelar.TabIndex = 3;
            this.cmbCancelar.Text = "Cancelar";
            this.cmbCancelar.UseVisualStyleBackColor = true;
            this.cmbCancelar.Click += new System.EventHandler(this.cmbCancelar_Click);
            // 
            // txtMunicipio
            // 
            this.txtMunicipio.Location = new System.Drawing.Point(2, 24);
            this.txtMunicipio.Name = "txtMunicipio";
            this.txtMunicipio.Size = new System.Drawing.Size(186, 20);
            this.txtMunicipio.TabIndex = 4;
            this.txtMunicipio.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMunicipio_KeyPress);
            // 
            // picFoto
            // 
            this.picFoto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(245)))));
            this.picFoto.Location = new System.Drawing.Point(255, 12);
            this.picFoto.Name = "picFoto";
            this.picFoto.Size = new System.Drawing.Size(66, 66);
            this.picFoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picFoto.TabIndex = 5;
            this.picFoto.TabStop = false;
            this.picFoto.DoubleClick += new System.EventHandler(this.picFoto_DoubleClick);
            // 
            // gridCatEscudo
            // 
            this.gridCatEscudo.AllowUserToAddRows = false;
            this.gridCatEscudo.AllowUserToDeleteRows = false;
            this.gridCatEscudo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridCatEscudo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gridCatEscudo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCatEscudo.Location = new System.Drawing.Point(6, 84);
            this.gridCatEscudo.Name = "gridCatEscudo";
            this.gridCatEscudo.Size = new System.Drawing.Size(475, 432);
            this.gridCatEscudo.TabIndex = 6;
            this.gridCatEscudo.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridCatEscudo_CellDoubleClick);
            // 
            // frmGridEscudos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 520);
            this.Controls.Add(this.gridCatEscudo);
            this.Controls.Add(this.picFoto);
            this.Controls.Add(this.txtMunicipio);
            this.Controls.Add(this.cmbCancelar);
            this.Controls.Add(this.cmbActualiza);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "frmGridEscudos";
            this.Text = "frmGridEscudos";
            this.Load += new System.EventHandler(this.frmGridEscudos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picFoto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCatEscudo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cmbActualiza;
        private System.Windows.Forms.Button cmbCancelar;
        private System.Windows.Forms.TextBox txtMunicipio;
        private System.Windows.Forms.PictureBox picFoto;
        private System.Windows.Forms.DataGridView gridCatEscudo;
    }
}