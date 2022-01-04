<?xml version="1.0" encoding="utf-8"?> 
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt"
				xmlns:b="http://schemas.openxmlformats.org/officeDocument/2006/bibliography" xmlns:t="http://www.microsoft.com/temp">
   <!--When the bibliography or citation is in your document, it's just HTML-->
   <xsl:output method="html" encoding="us-ascii"/> 
   <!--Match the root element, and dispatch to its children-->
   <xsl:template match="/"> 
      <xsl:apply-templates select="*" /> 
   </xsl:template>
   <!--Set an optional version number for this style-->
   <xsl:template match="b:version"> 
      <xsl:text>2021.1.01</xsl:text> 
   </xsl:template> 
   <!--Defines the name of the style in the References dropdown-->
   <xsl:template match="b:StyleName">     
      <xsl:text>WBST</xsl:text> 
   </xsl:template> 
   <!--Specifies which fields should appear in the Create Source dialog when in a collapsed state (The Show All Bibliography Fieldscheckbox is cleared)-->
   <xsl:template match="b:GetImportantFields[b:SourceType = 'Book']"> 
      <b:ImportantFields> 
         <b:ImportantField><xsl:text>b:Author/b:Author/b:NameList</xsl:text> </b:ImportantField> 
         <b:ImportantField> <xsl:text>b:Title</xsl:text> </b:ImportantField> 
         <b:ImportantField> <xsl:text>b:Year</xsl:text> </b:ImportantField> 
         <b:ImportantField> <xsl:text>b:City</xsl:text> </b:ImportantField> 
         <b:ImportantField> <xsl:text>b:Publisher</xsl:text> </b:ImportantField> 
      </b:ImportantFields> 
   </xsl:template>
   <!--Defines the output format for a simple Book (in the Bibliography) with important fields defined-->
   <xsl:template match="b:Source[b:SourceType = 'Book']">
   <!--Count the number of Corporate Authors (can only be 0 or 1-->
   <xsl:variable name="cCorporateAuthors">
      <xsl:value-of select="count(b:Author/b:Author/b:Corporate)" />
   </xsl:variable>
   <!--Label the paragraph as an Office Bibliography paragraph-->
   <p>
      <xsl:choose>
         <xsl:when test ="$cCorporateAuthors!=0">
         <!--When the corporate author exists display the corporate author-->
            <xsl:value-of select="b:Author/b:Author/b:Corporate"/>
            <xsl:text>. (</xsl:text>
         </xsl:when>
         <xsl:otherwise>
            <!--When the corporate author does not exist, display the normal author-->
            <xsl:value-of select="b:Author/b:Author/b:NameList/b:Person/b:Last"/>
            <xsl:text>, </xsl:text>
            <xsl:value-of select="b:Author/b:Author/b:NameList/b:Person/b:First"/>
            <xsl:text>. (</xsl:text>
         </xsl:otherwise>
      </xsl:choose>
      <xsl:value-of select="b:Year"/>
      <xsl:text>). </xsl:text>
      <i>
         <xsl:value-of select="b:Title"/>
         <xsl:text>. </xsl:text>
      </i> 
         <xsl:value-of select="b:City"/>
         <xsl:text>: </xsl:text>
         <xsl:value-of select="b:Publisher"/>
         <xsl:text>.</xsl:text>
      </p>
   </xsl:template>
   <!--Defines the output of the entire Bibliography-->
   <xsl:template match="b:Bibliography"> 
      <html xmlns="https://www.w3.org/TR/REC-html40"> 
         <body>
            <xsl:apply-templates select ="*">
            </xsl:apply-templates>
         </body>
      </html>
   </xsl:template>
   <!--Defines the output of the Citation-->
   <xsl:template match="b:Citation/b:Source[b:SourceType = 'Book']">
      <html xmlns="https://www.w3.org/TR/REC-html40"> 
         <xsl:variable name="cCorporateAuthors"> 
            <xsl:value-of select="count(b:Author/b:Author/b:Corporate)" /> 
         </xsl:variable> 
         <body> 
         <!--Defines the output format as (Author, Year--> 
            <xsl:text>(</xsl:text>
            <xsl:choose> 
            <!--When the corporate author exists display the corporate author-->
               <xsl:when test ="$cCorporateAuthors!=0">
                  <xsl:value-of select="b:Author/b:Author/b:Corporate"/>
               </xsl:when>
               <!--When the corporate author does not exist, display the normal author-->
               <xsl:otherwise> 
                  <xsl:value-of select="b:Author/b:Author/b:NameList/b:Person/b:Last"/> 
               </xsl:otherwise>
               </xsl:choose>
               <xsl:text>, </xsl:text> 
               <xsl:value-of select="b:Year"/>
               <xsl:text>)</xsl:text> 
            </body> 
         </html>
   </xsl:template>
   <xsl:template match="text()" />
</xsl:stylesheet>
