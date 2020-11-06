import { Component, OnInit } from '@angular/core';
import { ITextSelection } from 'src/classes/interface/ITextSelection';
import { C1TextSelection } from 'src/classes/base/C1TextSelection';

import * as $ from "jquery";
import { IC1FileInput } from 'src/classes/interface/IC1FileInput';
import { C1FileInput } from '../../classes/base/C1FileInput';
import { IC1Template } from 'src/classes/interface/IC1Template';
import { AlertController, ToastController, LoadingController, ModalController } from '@ionic/angular';
import { C1Template } from '../../classes/base/C1Template';
import { C1TemplateField } from '../../classes/base/C1TemplateField';
import { IC1TemplateField } from '../../classes/interface/IC1TemplateField';
import { IC1TemplateBlock } from '../../classes/interface/IC1TemplateBlock';
import { C1TemplateBlock } from '../../classes/base/C1TemlateBlock';

import { ExtractDataComponent } from '../components/extract-data/extract-data.component';
import { FileService } from '../services/file.service';
import { C1ParserService } from '../services/c1-parser.service';
import { C1TemplateBuilder } from '../../classes/base/C1TemplateBuilder';
import { ConfigService } from '../services/config.service';
import { C1Utils } from 'src/classes/base/C1Utils';
import { ParserService } from '../providers/parser.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: 'home.page.html',
  styleUrls: ['home.page.scss'],
})
export class HomePage implements OnInit {

  _EmailContent: string = '';
  _Selection: ITextSelection = new C1TextSelection();
  _Files: Array<IC1FileInput> = [];
  _SelectedFile: IC1FileInput = null;
  _Template: IC1Template = null;
  _OriginDocument: Document;
  _CurrentFileIndex: number = 0;
  _Templates: Array<IC1Template> = [];
  _Config = null;

  public constructor(public router: Router, public parserService: ParserService, public configService: ConfigService, public _FileService: FileService, public c1ParserService: C1ParserService, public modalController: ModalController, public loadingController: LoadingController, public alertController: AlertController, public toastController: ToastController) {
    this.configService.loadConfig().then(data => {
      this._Config = this.configService.getData();
    }, error => { });
  }

  filesChecked: boolean = false;
  onFilesCheckboxValueChanged() {
    this._Files.forEach(item => {
      item.checked = this.filesChecked;
    })
  }

  onClickSelectTemplate(template: IC1Template) {
    this._Templates.forEach(tmp => {
      if (tmp.getName() != template.getName()) { tmp.checked = false; }
    });
  }


  onResponseDefaultFiles(arr) {
    if (!arr || arr.length == 0) return;
    arr.forEach(filePath => {
      if (filePath.indexOf(".html") != -1) {
        this._FileService.loadFile(this.c1ParserService.API_URL + filePath, (file) => {
          if (file) {
            this._AddEmailFromFile(new C1FileInput(file));
          }
        });
      }

      if (filePath.indexOf(".xml") != -1) {
        this._FileService.loadFile(this.c1ParserService.API_URL + filePath, (file) => {
          if (file) {
            this._AddTemplateFromFile(file);
          }
        });
      }
    });
  }

  ngOnInit(): void {
    this.c1ParserService.load().then(loadedData => {
      this.c1ParserService.getListDefaultFiles().then(res => {
        this.onResponseDefaultFiles(res);
      }, error => { });
    });
  }

  onClickPreviousFile() {
    this._CurrentFileIndex -= 1;
    if (this._CurrentFileIndex < 0) this._CurrentFileIndex = 0;
    this._OpenEmail(this._Files[this._CurrentFileIndex]);
  }

  onClickNextFile() {
    this._CurrentFileIndex += 1;
    if (this._CurrentFileIndex > this._Files.length - 1) this._CurrentFileIndex = this._Files.length - 1;
    this._OpenEmail(this._Files[this._CurrentFileIndex]);
  }

  public onClickCreateTemplate(file: IC1FileInput) {
    this.onClickAddNewTemplate(file);
  }

  async onClickExtractData(file: IC1FileInput, template?: IC1Template) {
    if (!file) {
      if (this._Files.length == 0) {
        this._PresentToast("There is no email to extract data!");
        return;
      } else {
        this._PresentAlertSelectEmail(template);
        return;
      }
    } else if (!template) {
      this._PresentAlertSelectTemplate(file);
    } else {
      await this._ExtractData(file, template);
    }
  }

  public onClickAddRepeatedField(field: IC1TemplateField) {
    if (!this._Template) return;
    if (!field) return;
    for (let f of field._RepeatedFields) f.dispose();
    let repeatedField = new C1TemplateField(field._Name, this._Selection);
    field._RepeatedFields.push(repeatedField);
    this._Selection = new C1TextSelection(this._OriginDocument);
  }

  onEmailFileChanged(event) {
    let notSupportedFiles = [];
    if (event.target.files && event.target.files.length) {
      for (let i = 0; i < event.target.files.length; i++) {
        let file: File = event.target.files[i];
        if (file && file.name.indexOf(".html") != -1) { this._AddEmailFromFile(new C1FileInput(file)); } else { notSupportedFiles.push(file); }
      }
      this._Files.sort((a: IC1FileInput, b: IC1FileInput) => {
        return a.getName().localeCompare(b.getName());
      });

      event.target.value = '';
    }

    if (notSupportedFiles.length) {
      let message: string = "These files are not supported: <br>";
      for (let fileNotSupport of notSupportedFiles) {
        message += `<div style="padding-top:10px"> <strong>${fileNotSupport.name}</strong></div>`;
      }
      this._PresentAlertConfirm("Warning!", message);
    }
  }

  onTemplateFileChanged(event) {
    let notSupportedFiles = [];
    if (event.target.files && event.target.files.length) {
      for (let i = 0; i < event.target.files.length; i++) {
        let file: File = event.target.files[i];
        if (file && file.name.indexOf(".xml") != -1) { this._AddTemplateFromFile(file); } else { notSupportedFiles.push(file); }
      }
      event.target.value = '';
    }

    if (notSupportedFiles.length) {
      let message: string = "These files are not supported: <br>";
      for (let fileNotSupport of notSupportedFiles) {
        message += `<div style="padding-top:10px"> <strong>${fileNotSupport.name}</strong></div>`;
      }
      this._PresentAlertConfirm("Warning!", message);
    }
  }

  private _PrettyContent(template: IC1Template, file: IC1FileInput, result): string {
    if (!result) return '';
    let prettyContent: string = '';
    prettyContent += `<h3> ${file.getName()} using ${template.getName()} template</h3>`;
    prettyContent += `<br>`;
    prettyContent += `<pre>${C1Utils.beautyJSON(JSON.stringify(result, null, 4))}</pre>`;
    return prettyContent;
  }


  async _DisplayExtractionResult(template: IC1Template, file: IC1FileInput, content: string) {
    let prettyContent: string = content;
    try {
      let obj = JSON.parse(content);
      if (obj) {
        prettyContent = this._PrettyContent(template, file, obj);
      }
    } catch (error) {

    }
    const modal = await this.modalController.create({
      component: ExtractDataComponent,
      componentProps: {
        content: prettyContent
      }
    });

    return modal.present();
  }

  onMouseUp(event) {
    if (!event) return;
    this._Selection = new C1TextSelection(this._OriginDocument, event);
    let windowSelection = C1Utils.getWindowSelection();
    if (!windowSelection) return;

    this._Selection.onSelection(windowSelection);

    if (this._Selection.isContainsTemplateFields()) {
      this._Selection = new C1TextSelection();
      windowSelection.removeAllRanges();
      this._PresentToast("Cannot add new field that contains another field!", true);
      return;
    }

    this._Selection.reproceduceEvents(this._OriginDocument, event);

  }


  onClickCloseFile() {
    this._SelectedFile = null;
    this._Template = null;
  }

  onClickSelectEmail(file: IC1FileInput) {
    this._OpenEmail(file);
  }

  onClickAddField() {
    this._PresentAlertCreateNewField();
  }

  onClickAddBlock() {
    this._PresentAlertCreateNewBlock();
  }

  onClickAddBlockField(block: IC1TemplateBlock) {
    this._PresentAlertCreateNewBlockField(block);
  }

  onClickAddNewTemplate(file?: IC1FileInput) {
    this._PresentAlertCreateTemplate(file);
  }

  public onClickEditField(field: IC1TemplateField) {
    if (!field) return;
    this._PresentAlertUpdateField(field);
  }

  public onClickDeleteField(field: IC1TemplateField) {
    if (!field) return;
    field.dispose();
    this._Template.removeField(field);
  }

  public onClickEditBlock(block: IC1TemplateBlock) {
    if (!block) return;
    this._PresentAlertUpdateBlock(block);
  }

  public onClickDeleteBlock(block: IC1TemplateBlock) {
    if (!block) return;
    this._Template.removeBlock(block);
  }

  public onClickEditBlockField(block: IC1TemplateBlock, field: IC1TemplateField) {
    if (!field) return;
    this._PresentAlertUpdateField(field);
  }

  public onClickDeleteBlockField(block: IC1TemplateBlock, field: IC1TemplateField) {
    if (!block || !field) return;
    field.dispose();
    block.removeField(field);
  }
  async onClickDeleteTemplate(template: IC1Template) {
    if (!template) return;

    const confirmAlert = await this.alertController.create({
      header: 'Confirm!',
      message: `Remove <strong>${template._Name}</strong> ?`,
      buttons: [
        {
          text: 'Cancel',
          role: 'cancel',
          cssClass: 'secondary',
          handler: (blah) => {
          }
        }, {
          text: 'OK',
          handler: () => {
            let index = this._Templates.indexOf(template);
            if (index != -1) {
              this._Templates.splice(index, 1);
            }
          }
        }
      ]
    });

    await confirmAlert.present();
  }

  public onClickSaveTemplate() {
    if (!this._Template || (this._Template.getFields().length + this._Template.getBlocks().length) == 0) {
      this._PresentToast("Please dont save an empty template!");
      return;
    }

    if (this.isEditingTemplate) {
      this.isEditingTemplate = false;
      let index = -1;
      for (let i = 0; i < this._Templates.length; i++) {
        if (this._Templates[i].getName() == this._Template.getName()) {
          index = i;
          break;
        }
      }
      if (index != -1) {
        this._Template._XMLData = "";
        this._Template.writeXML();
        this._Templates[index] = this._Template;
        console.log("Index ", index);
      }

      this.onClickDiscardEditTemplate();
      return;
    }

    this._Templates.push(this._Template);
    this.onClickDiscardEditTemplate();
  }

  private async _PresentAlertCreateNewField() {
    const alert = await this.alertController.create({
      header: 'Create new field!',
      inputs: [
        {
          name: 'name',
          type: 'text',
          placeholder: 'Field name',
          value: 'field ' + this._guidGenerator()
        }
      ],
      buttons: [
        {
          text: 'Cancel',
          role: 'cancel',
          cssClass: 'secondary',
          handler: () => {
          }
        }, {
          text: 'Ok',
          handler: (val) => {
            if (val && val.name) {
              this._AddNewField(val.name);
            }
          }
        }
      ]
    });

    await alert.present();
  }

  private async _PresentAlertUpdateField(field: IC1TemplateField) {
    const alert = await this.alertController.create({
      header: 'Update Field name!',
      inputs: [
        {
          name: 'name',
          type: 'text',
          placeholder: 'Field name',
          value: field._Name
        }
      ],
      buttons: [
        {
          text: 'Cancel',
          role: 'cancel',
          cssClass: 'secondary',
          handler: () => {
          }
        }, {
          text: 'Ok',
          handler: (val) => {
            if (val && val.name) {
              field.setName(val.name);
            } else {
              this._PresentToast("name is not valid");
            }
          }
        }
      ]
    });

    await alert.present();
  }

  private async _PresentAlertUpdateBlock(block: IC1TemplateBlock) {
    const alert = await this.alertController.create({
      header: 'Update block name!',
      inputs: [
        {
          name: 'name',
          type: 'text',
          placeholder: 'block name',
          value: block._Name
        }
      ],
      buttons: [
        {
          text: 'Cancel',
          role: 'cancel',
          cssClass: 'secondary',
          handler: () => {
          }
        }, {
          text: 'Ok',
          handler: (val) => {
            if (val && val.name) {
              if (val.name != block.getName()) {
                if (this._Template.getBlockByName(val.name)) {
                  this._PresentToast("Invalid name!", true);
                  return;
                }
              }
              block.setName(val.name);
            } else {
              this._PresentToast("name is not valid");
            }
          }
        }
      ]
    });

    await alert.present();
  }

  private _AddNewField(name: string) {
    if (!this._Template) return;
    if (this._Template.getFieldByName(name)) {
      this._PresentToast("Invalid field name, dupplicated!");
      return;
    }
    {
      var target = this._Selection._Target;
      if (target) {
        if (!target.getAttribute('c1shadowdata')) {
          target.setAttribute('c1shadowdata', target.textContent);
        }
      }
    }
    let field = new C1TemplateField(name, this._Selection);
    this._Template.addField(field);
    this._Selection = new C1TextSelection(this._OriginDocument);
  }

  private _AddNewBlockField(block: IC1TemplateBlock, fieldName: string) {
    if (!this._Template) return;
    if (!block) return;
    let fd = block.getFieldByName(fieldName);
    if (fd) {
      for (let f of fd._RepeatedFields) f.dispose();
      let repeatedField = new C1TemplateField(fd.getName(), this._Selection);
      fd._RepeatedFields.push(repeatedField);
      this._Selection = new C1TextSelection(this._OriginDocument);
      return;
    }
    let field = new C1TemplateField(fieldName, this._Selection);
    block.addField(field);
    this._Selection = new C1TextSelection(this._OriginDocument);
  }

  private _AddNewBlock(name: string) {
    if (!this._Template) return;
    if (this._Template.getBlockByName(name)) {
      this._PresentToast("This repeated block name has been exist, please try another name!", true);
      return;
    }
    let block: IC1TemplateBlock = new C1TemplateBlock(name);
    this._Template.addBlock(block);
  }

  private async _PresentAlertConfirm(title: string, message: string) {
    const confirmAlert = await this.alertController.create({
      header: title,
      message: message,
      buttons: [
        {
          text: 'OK',
          handler: () => { }
        }
      ]
    });
    await confirmAlert.present();
  }
  private async _PresentAlertCreateNewBlockField(block: IC1TemplateBlock) {
    const alert = await this.alertController.create({
      header: 'Create new field for ' + block.getName() + '!',
      inputs: [
        {
          name: 'name',
          type: 'text',
          placeholder: 'field name',
          value: 'field ' + this._guidGenerator()
        }

      ],
      buttons: [
        {
          text: 'Cancel',
          role: 'cancel',
          cssClass: 'secondary',
          handler: () => {
          }
        }, {
          text: 'Ok',
          handler: (val) => {
            if (val && val.name) {
              this._AddNewBlockField(block, val.name);
            }
          }
        }
      ]
    });
    await alert.present();
  }

  private async _PresentAlertCreateNewBlock() {
    const alert = await this.alertController.create({
      header: 'Create new block!',
      inputs: [
        {
          name: 'name',
          type: 'text',
          placeholder: 'Block name',
          value: 'block ' + this._guidGenerator()
        }

      ],
      buttons: [
        {
          text: 'Cancel',
          role: 'cancel',
          cssClass: 'secondary',
          handler: () => {
          }
        }, {
          text: 'Ok',
          handler: (val) => {
            if (val && val.name) {
              this._AddNewBlock(val.name);
            }
          }
        }
      ]
    });
    await alert.present();
  }

  private async _PresentAlertCreateTemplate(file?: IC1FileInput) {
    const alert = await this.alertController.create({
      header: 'Create template!',
      inputs: [
        {
          name: 'name',
          type: 'text',
          placeholder: 'Template name',
          value: 'template ' + this._guidGenerator()
        }
      ],
      buttons: [
        {
          text: 'Cancel',
          role: 'cancel',
          cssClass: 'secondary',
          handler: () => {
          }
        }, {
          text: 'Ok',
          handler: (val) => {
            if (val && val.name) {
              if (file) {
                this._OpenEmail(file);
              }

              this._Template = new C1Template(val.name);
              this._Template["_Version"] = this._Config.version;
              this._Template._Source = this._EmailContent;
            }
          }
        }
      ]
    });

    await alert.present();
  }

  private async _PresentAlertSelectTemplate(file: IC1FileInput) {
    if (this._Templates.length == 0) {
      const alert = await this.alertController.create({
        header: 'There is no template, create one?',
        buttons: [
          {
            text: 'Cancel',
            role: 'cancel',
            cssClass: 'secondary',
            handler: () => {
            }
          }, {
            text: 'Ok',
            handler: (val) => {
              this._PresentAlertCreateTemplate(file);
            }
          }
        ]
      });

      await alert.present();

      return;
    }

    let templateInputs = [];
    for (let i = 0; i < this._Templates.length; i++) {
      let template = this._Templates[i];
      templateInputs.push({
        name: template._Name,
        type: 'radio',
        label: template._Name,
        value: template._Name,
        checked: i == 0
      });
    }

    const alert = await this.alertController.create({
      header: 'Select template for ' + file._Name,
      inputs: templateInputs,
      buttons: [
        {
          text: 'Cancel',
          role: 'cancel',
          cssClass: 'secondary',
          handler: () => {
          }
        }, {
          text: 'Ok',
          handler: (val) => {
            if (val) {
              let template = this._Templates.find(ele => {
                return ele._Name == val;
              });
              if (template) {
                this._ExtractData(file, template);
              }
            }
          }
        }
      ]
    });

    await alert.present();
  }

  private async _PresentAlertSelectEmail(template: IC1Template) {

    let fileInputs = [];
    for (let i = 0; i < this._Files.length; i++) {
      let fileInput = this._Files[i];
      fileInputs.push({
        name: fileInput._Name,
        type: 'radio',
        label: fileInput._Name,
        value: fileInput._Name,
        checked: i == 0
      });
    }

    const alert = await this.alertController.create({
      header: 'Select email for ' + template._Name,
      inputs: fileInputs,
      buttons: [
        {
          text: 'Cancel',
          role: 'cancel',
          cssClass: 'secondary',
          handler: () => {
          }
        }, {
          text: 'Ok',
          handler: (val) => {
            if (val) {
              let file = this._Files.find(ele => {
                return ele._Name == val;
              });
              if (file) {
                this._ExtractData(file, template);
              }
            }
          }
        }
      ]
    });

    await alert.present();
  }



  private _guidGenerator() {
    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
  }

  async _PresentToast(message: string, isMiddle?: boolean) {
    const toast = await this.toastController.create({
      message: message,
      duration: 2000,
      position: isMiddle ? 'middle' : 'bottom'
    });
    toast.present();
  }

  public _AddEmailFromFile(file: IC1FileInput) {
    if (!file || file.getName().length == 0) return;
    let c1FileInput = this._Files.find(ele => {
      return ele.getID().localeCompare(file.getID()) == 0;
    });
    if (!c1FileInput) {
      this._Files.push(file);
      this._Files.sort((a: IC1FileInput, b: IC1FileInput) => {
        return a.getName().localeCompare(b.getName());
      });
    }
  }

  private _AddTemplateFromFile(file: File) {
    if (!file) return;
    let reader = new FileReader();
    reader.onload = () => {
      let templatename = file.name.replace('.xml', '');
      let template = new C1Template(templatename);
      var result = reader.result.toString();
      template._XMLData = result;

      let exist = this._Templates.find(ele => {
        return ele.getName() == templatename;
      });
      if (!exist) {
        this._Templates.push(template);
      }
    };
    reader.readAsText(file);
  }

  private _OpenEmail(file: IC1FileInput) {
    this._SelectedFile = file;
    this._CurrentFileIndex = this._Files.indexOf(file);
    let reader = new FileReader();
    reader.onload = () => {
      var result = reader.result.toString();
      result = result.replace(/\r\n/g, "\n");
      this._EmailContent = result;
      if (this._Template) {
        this._Template._Source = result;
        this._Template.setFile(file);
      }
      $('#contents').html(result);
      var domParser = new DOMParser();
      this._OriginDocument = domParser.parseFromString(this._EmailContent, "text/html");
    };
    reader.readAsText(file.getFile());
    this._Template = null;
  }

  onClickDiscardEditTemplate() {
    this._Template = null;
    this._SelectedFile = null;
  }

  onClickDownloadTemplate(template: IC1Template) {
    C1Utils.saveTemplateToFile(template);
  }

  isEditingTemplate: boolean = false;
  async presentAlertConfirm(header, message) {
    const alert = await this.alertController.create({
      header: header,
      message: message,
      buttons: [
        {
          text: 'Ok',
          handler: () => {
          }
        }
      ]
    });
    await alert.present();
  }

  onClickEditTemplate(template: IC1Template) {
    if (!template) return;
    let templateBuilder = new C1TemplateBuilder();
    templateBuilder.build(template).then(() => {
      // Check template version, if the template does not contains version information, it means we do not support edit feature on this template
      if (!templateBuilder.getTemplateVersion()) {
        this.presentAlertConfirm("Warning!", "This template was built with older version that does not support edit feature!");
        return;
      }

      this.isEditingTemplate = true;

      let result = templateBuilder.getHTMLContentString();
      result = result.replace(/\r\n/g, "\n");
      this._EmailContent = result;
      $('#contents').html(result);


      let domParser = new DOMParser();
      this._OriginDocument = domParser.parseFromString(this._EmailContent, "text/html");

      let sourceFileName = templateBuilder.getEmailName();
      let file = new File([result], sourceFileName ? sourceFileName : "email.html", { type: "text/html" })
      this._SelectedFile = new C1FileInput(file);
      this._CurrentFileIndex = -1;

      this._Template = new C1Template(templateBuilder.getTemplateName());
      this._Template["_Version"] = templateBuilder.getTemplateVersion();

      if (!this._Template._Source) this._Template._Source = result;
      this._Template.setFile(this._SelectedFile);
      templateBuilder.rebuildFields(this._Template, document);
      this._Template._XMLData = "";
      // Remove selection
      if (window.getSelection) {
        window.getSelection().removeAllRanges();
      } else if (document['selection']) {
        document['selection'].removeAllRanges();
      }

    }, err => {
      console.log("Parse Error", err);
    });
  }

  async _ExtractData(file: IC1FileInput, template: IC1Template) {

    const loading = await this.loadingController.create({
      message: 'Please wait..',
      duration: 10000
    });
    await loading.present();
    this.c1ParserService.load().then(res => {
      this.c1ParserService.extractData(template, file).then(
        (data) => {
          this._DisplayExtractionResult(template, file, data).then(() => {
            loading.dismiss();
          }).catch(err => {
            loading.dismiss();
          });
        }, (err) => {
          loading.dismiss();
          this._PresentToast(err);
        });
    }, err => {
      this._PresentToast(err);
    });

  }


  onClickExtractDatas() {

    let emails: Array<IC1FileInput> = this._Files.filter(ele => {
      return ele.checked;
    });
    if (emails.length == 0) {
      let message = "Please select at least 1 email to extract data";
      if (this._Files.length == 0) {
        message = "There is no email to extract, please add a new one";
      }
      this.presentAlertConfirm("Warning!", message);
      return;
    }
    let template: IC1Template = this._Templates.find(ele => {
      return ele.checked;
    });

    if (!template) {
      let message = "Please select template to extract data";
      if (this._Templates.length == 0) {
        message = "There is no template, please create new one";
      }
      this.presentAlertConfirm("Warning!", message);
      return;
    }

    this.parserService.getParser().setService(this.c1ParserService);
    this.parserService.getParser().setTasks(emails, template);
    this.router.navigate(["/home/parser"]);
  }
}
