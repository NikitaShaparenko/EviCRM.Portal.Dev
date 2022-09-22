var pinnedpath;

function readURL(input) {
  
    
}

function extensionParser(inf)
{
	return (inf.toString().substring(inf.toString().lastIndexOf('.')+1, inf.length) || filename);
}

function isValidUri(inputPath){
var extens = extensionParser(inputPath);

if (extens == "xlsx")
{
	return true;
}
else if (extens == "xls")
{
	return true;
}
else if (extens == "csv")
{
	return true;
}
else if (extens == "doc")
{
	return true;
}
else if (extens == "docx")
{
	return true;
}
else
{
	return false;
}
}

        function checkFileExtension() {
            fileName = document.querySelector('#fieSelect').value;
            extension = fileName.split('.').pop();
            document.querySelector('.output')
                                     .textContent = extension;
        };

function PinFile(inputPath)
{
	
	if (isValidUri(inputPath))
	{
			pinnedpath=inputPath;
			document.getElementById('image-title').innerHTML=pinnedpath;
			removeUpload();
	}
	else
	{
		alert("Выберите офисный документ с одним из следующих расширений: ( doc, docx - Microsoft Word ), ( csv, xls, xlsx - Microsoft Excel ) !");
	}
}



function removeUpload() {
$('.drag-text').hide();
  $('.file-upload-btn').hide();
  $('.image-upload-wrap').hide();
  $('.file-upload-content').show();
  $('.image-title-wrap').show();
  $('.remove-image').show();
  $('.remove-image2').show();
}

function btnBack(){
	 $('.drag-text').show();
$('.file-upload-btn').show();
  $('.image-upload-wrap').show();
 $('.file-upload-content').hide();
 $('.image-title-wrap').hide(); 
 $('.remove-image').hide();
  $('.remove-image2').hide();
}