 

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Traction.Roslyn.Rewriting {

    partial class CompositeRewriter {

        public override SyntaxNode VisitAttributeArgument(AttributeArgumentSyntax node) {
                
            AttributeArgumentSyntax result = node;
            foreach (var item in items) {
                result = (AttributeArgumentSyntax)VisitAttributeArgument(result);
            }
            return result;
        }

    public override SyntaxNode VisitNameEquals(NameEqualsSyntax node) {
                
            NameEqualsSyntax result = node;
            foreach (var item in items) {
                result = (NameEqualsSyntax)VisitNameEquals(result);
            }
            return result;
        }

    public override SyntaxNode VisitTypeParameterList(TypeParameterListSyntax node) {
                
            TypeParameterListSyntax result = node;
            foreach (var item in items) {
                result = (TypeParameterListSyntax)VisitTypeParameterList(result);
            }
            return result;
        }

    public override SyntaxNode VisitTypeParameter(TypeParameterSyntax node) {
                
            TypeParameterSyntax result = node;
            foreach (var item in items) {
                result = (TypeParameterSyntax)VisitTypeParameter(result);
            }
            return result;
        }

    public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node) {
                
            ClassDeclarationSyntax result = node;
            foreach (var item in items) {
                result = (ClassDeclarationSyntax)VisitClassDeclaration(result);
            }
            return result;
        }

    public override SyntaxNode VisitStructDeclaration(StructDeclarationSyntax node) {
                
            StructDeclarationSyntax result = node;
            foreach (var item in items) {
                result = (StructDeclarationSyntax)VisitStructDeclaration(result);
            }
            return result;
        }

    public override SyntaxNode VisitInterfaceDeclaration(InterfaceDeclarationSyntax node) {
                
            InterfaceDeclarationSyntax result = node;
            foreach (var item in items) {
                result = (InterfaceDeclarationSyntax)VisitInterfaceDeclaration(result);
            }
            return result;
        }

    public override SyntaxNode VisitEnumDeclaration(EnumDeclarationSyntax node) {
                
            EnumDeclarationSyntax result = node;
            foreach (var item in items) {
                result = (EnumDeclarationSyntax)VisitEnumDeclaration(result);
            }
            return result;
        }

    public override SyntaxNode VisitDelegateDeclaration(DelegateDeclarationSyntax node) {
                
            DelegateDeclarationSyntax result = node;
            foreach (var item in items) {
                result = (DelegateDeclarationSyntax)VisitDelegateDeclaration(result);
            }
            return result;
        }

    public override SyntaxNode VisitEnumMemberDeclaration(EnumMemberDeclarationSyntax node) {
                
            EnumMemberDeclarationSyntax result = node;
            foreach (var item in items) {
                result = (EnumMemberDeclarationSyntax)VisitEnumMemberDeclaration(result);
            }
            return result;
        }

    public override SyntaxNode VisitBaseList(BaseListSyntax node) {
                
            BaseListSyntax result = node;
            foreach (var item in items) {
                result = (BaseListSyntax)VisitBaseList(result);
            }
            return result;
        }

    public override SyntaxNode VisitSimpleBaseType(SimpleBaseTypeSyntax node) {
                
            SimpleBaseTypeSyntax result = node;
            foreach (var item in items) {
                result = (SimpleBaseTypeSyntax)VisitSimpleBaseType(result);
            }
            return result;
        }

    public override SyntaxNode VisitTypeParameterConstraintClause(TypeParameterConstraintClauseSyntax node) {
                
            TypeParameterConstraintClauseSyntax result = node;
            foreach (var item in items) {
                result = (TypeParameterConstraintClauseSyntax)VisitTypeParameterConstraintClause(result);
            }
            return result;
        }

    public override SyntaxNode VisitConstructorConstraint(ConstructorConstraintSyntax node) {
                
            ConstructorConstraintSyntax result = node;
            foreach (var item in items) {
                result = (ConstructorConstraintSyntax)VisitConstructorConstraint(result);
            }
            return result;
        }

    public override SyntaxNode VisitClassOrStructConstraint(ClassOrStructConstraintSyntax node) {
                
            ClassOrStructConstraintSyntax result = node;
            foreach (var item in items) {
                result = (ClassOrStructConstraintSyntax)VisitClassOrStructConstraint(result);
            }
            return result;
        }

    public override SyntaxNode VisitTypeConstraint(TypeConstraintSyntax node) {
                
            TypeConstraintSyntax result = node;
            foreach (var item in items) {
                result = (TypeConstraintSyntax)VisitTypeConstraint(result);
            }
            return result;
        }

    public override SyntaxNode VisitFieldDeclaration(FieldDeclarationSyntax node) {
                
            FieldDeclarationSyntax result = node;
            foreach (var item in items) {
                result = (FieldDeclarationSyntax)VisitFieldDeclaration(result);
            }
            return result;
        }

    public override SyntaxNode VisitEventFieldDeclaration(EventFieldDeclarationSyntax node) {
                
            EventFieldDeclarationSyntax result = node;
            foreach (var item in items) {
                result = (EventFieldDeclarationSyntax)VisitEventFieldDeclaration(result);
            }
            return result;
        }

    public override SyntaxNode VisitExplicitInterfaceSpecifier(ExplicitInterfaceSpecifierSyntax node) {
                
            ExplicitInterfaceSpecifierSyntax result = node;
            foreach (var item in items) {
                result = (ExplicitInterfaceSpecifierSyntax)VisitExplicitInterfaceSpecifier(result);
            }
            return result;
        }

    public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node) {
                
            MethodDeclarationSyntax result = node;
            foreach (var item in items) {
                result = (MethodDeclarationSyntax)VisitMethodDeclaration(result);
            }
            return result;
        }

    public override SyntaxNode VisitOperatorDeclaration(OperatorDeclarationSyntax node) {
                
            OperatorDeclarationSyntax result = node;
            foreach (var item in items) {
                result = (OperatorDeclarationSyntax)VisitOperatorDeclaration(result);
            }
            return result;
        }

    public override SyntaxNode VisitConversionOperatorDeclaration(ConversionOperatorDeclarationSyntax node) {
                
            ConversionOperatorDeclarationSyntax result = node;
            foreach (var item in items) {
                result = (ConversionOperatorDeclarationSyntax)VisitConversionOperatorDeclaration(result);
            }
            return result;
        }

    public override SyntaxNode VisitConstructorDeclaration(ConstructorDeclarationSyntax node) {
                
            ConstructorDeclarationSyntax result = node;
            foreach (var item in items) {
                result = (ConstructorDeclarationSyntax)VisitConstructorDeclaration(result);
            }
            return result;
        }

    public override SyntaxNode VisitConstructorInitializer(ConstructorInitializerSyntax node) {
                
            ConstructorInitializerSyntax result = node;
            foreach (var item in items) {
                result = (ConstructorInitializerSyntax)VisitConstructorInitializer(result);
            }
            return result;
        }

    public override SyntaxNode VisitDestructorDeclaration(DestructorDeclarationSyntax node) {
                
            DestructorDeclarationSyntax result = node;
            foreach (var item in items) {
                result = (DestructorDeclarationSyntax)VisitDestructorDeclaration(result);
            }
            return result;
        }

    public override SyntaxNode VisitPropertyDeclaration(PropertyDeclarationSyntax node) {
                
            PropertyDeclarationSyntax result = node;
            foreach (var item in items) {
                result = (PropertyDeclarationSyntax)VisitPropertyDeclaration(result);
            }
            return result;
        }

    public override SyntaxNode VisitArrowExpressionClause(ArrowExpressionClauseSyntax node) {
                
            ArrowExpressionClauseSyntax result = node;
            foreach (var item in items) {
                result = (ArrowExpressionClauseSyntax)VisitArrowExpressionClause(result);
            }
            return result;
        }

    public override SyntaxNode VisitEventDeclaration(EventDeclarationSyntax node) {
                
            EventDeclarationSyntax result = node;
            foreach (var item in items) {
                result = (EventDeclarationSyntax)VisitEventDeclaration(result);
            }
            return result;
        }

    public override SyntaxNode VisitIndexerDeclaration(IndexerDeclarationSyntax node) {
                
            IndexerDeclarationSyntax result = node;
            foreach (var item in items) {
                result = (IndexerDeclarationSyntax)VisitIndexerDeclaration(result);
            }
            return result;
        }

    public override SyntaxNode VisitAccessorList(AccessorListSyntax node) {
                
            AccessorListSyntax result = node;
            foreach (var item in items) {
                result = (AccessorListSyntax)VisitAccessorList(result);
            }
            return result;
        }

    public override SyntaxNode VisitAccessorDeclaration(AccessorDeclarationSyntax node) {
                
            AccessorDeclarationSyntax result = node;
            foreach (var item in items) {
                result = (AccessorDeclarationSyntax)VisitAccessorDeclaration(result);
            }
            return result;
        }

    public override SyntaxNode VisitParameterList(ParameterListSyntax node) {
                
            ParameterListSyntax result = node;
            foreach (var item in items) {
                result = (ParameterListSyntax)VisitParameterList(result);
            }
            return result;
        }

    public override SyntaxNode VisitBracketedParameterList(BracketedParameterListSyntax node) {
                
            BracketedParameterListSyntax result = node;
            foreach (var item in items) {
                result = (BracketedParameterListSyntax)VisitBracketedParameterList(result);
            }
            return result;
        }

    public override SyntaxNode VisitParameter(ParameterSyntax node) {
                
            ParameterSyntax result = node;
            foreach (var item in items) {
                result = (ParameterSyntax)VisitParameter(result);
            }
            return result;
        }

    public override SyntaxNode VisitIncompleteMember(IncompleteMemberSyntax node) {
                
            IncompleteMemberSyntax result = node;
            foreach (var item in items) {
                result = (IncompleteMemberSyntax)VisitIncompleteMember(result);
            }
            return result;
        }

    public override SyntaxNode VisitSkippedTokensTrivia(SkippedTokensTriviaSyntax node) {
                
            SkippedTokensTriviaSyntax result = node;
            foreach (var item in items) {
                result = (SkippedTokensTriviaSyntax)VisitSkippedTokensTrivia(result);
            }
            return result;
        }

    public override SyntaxNode VisitDocumentationCommentTrivia(DocumentationCommentTriviaSyntax node) {
                
            DocumentationCommentTriviaSyntax result = node;
            foreach (var item in items) {
                result = (DocumentationCommentTriviaSyntax)VisitDocumentationCommentTrivia(result);
            }
            return result;
        }

    public override SyntaxNode VisitTypeCref(TypeCrefSyntax node) {
                
            TypeCrefSyntax result = node;
            foreach (var item in items) {
                result = (TypeCrefSyntax)VisitTypeCref(result);
            }
            return result;
        }

    public override SyntaxNode VisitQualifiedCref(QualifiedCrefSyntax node) {
                
            QualifiedCrefSyntax result = node;
            foreach (var item in items) {
                result = (QualifiedCrefSyntax)VisitQualifiedCref(result);
            }
            return result;
        }

    public override SyntaxNode VisitNameMemberCref(NameMemberCrefSyntax node) {
                
            NameMemberCrefSyntax result = node;
            foreach (var item in items) {
                result = (NameMemberCrefSyntax)VisitNameMemberCref(result);
            }
            return result;
        }

    public override SyntaxNode VisitIndexerMemberCref(IndexerMemberCrefSyntax node) {
                
            IndexerMemberCrefSyntax result = node;
            foreach (var item in items) {
                result = (IndexerMemberCrefSyntax)VisitIndexerMemberCref(result);
            }
            return result;
        }

    public override SyntaxNode VisitOperatorMemberCref(OperatorMemberCrefSyntax node) {
                
            OperatorMemberCrefSyntax result = node;
            foreach (var item in items) {
                result = (OperatorMemberCrefSyntax)VisitOperatorMemberCref(result);
            }
            return result;
        }

    public override SyntaxNode VisitConversionOperatorMemberCref(ConversionOperatorMemberCrefSyntax node) {
                
            ConversionOperatorMemberCrefSyntax result = node;
            foreach (var item in items) {
                result = (ConversionOperatorMemberCrefSyntax)VisitConversionOperatorMemberCref(result);
            }
            return result;
        }

    public override SyntaxNode VisitCrefParameterList(CrefParameterListSyntax node) {
                
            CrefParameterListSyntax result = node;
            foreach (var item in items) {
                result = (CrefParameterListSyntax)VisitCrefParameterList(result);
            }
            return result;
        }

    public override SyntaxNode VisitCrefBracketedParameterList(CrefBracketedParameterListSyntax node) {
                
            CrefBracketedParameterListSyntax result = node;
            foreach (var item in items) {
                result = (CrefBracketedParameterListSyntax)VisitCrefBracketedParameterList(result);
            }
            return result;
        }

    public override SyntaxNode VisitCrefParameter(CrefParameterSyntax node) {
                
            CrefParameterSyntax result = node;
            foreach (var item in items) {
                result = (CrefParameterSyntax)VisitCrefParameter(result);
            }
            return result;
        }

    public override SyntaxNode VisitXmlElement(XmlElementSyntax node) {
                
            XmlElementSyntax result = node;
            foreach (var item in items) {
                result = (XmlElementSyntax)VisitXmlElement(result);
            }
            return result;
        }

    public override SyntaxNode VisitXmlElementStartTag(XmlElementStartTagSyntax node) {
                
            XmlElementStartTagSyntax result = node;
            foreach (var item in items) {
                result = (XmlElementStartTagSyntax)VisitXmlElementStartTag(result);
            }
            return result;
        }

    public override SyntaxNode VisitXmlElementEndTag(XmlElementEndTagSyntax node) {
                
            XmlElementEndTagSyntax result = node;
            foreach (var item in items) {
                result = (XmlElementEndTagSyntax)VisitXmlElementEndTag(result);
            }
            return result;
        }

    public override SyntaxNode VisitXmlEmptyElement(XmlEmptyElementSyntax node) {
                
            XmlEmptyElementSyntax result = node;
            foreach (var item in items) {
                result = (XmlEmptyElementSyntax)VisitXmlEmptyElement(result);
            }
            return result;
        }

    public override SyntaxNode VisitXmlName(XmlNameSyntax node) {
                
            XmlNameSyntax result = node;
            foreach (var item in items) {
                result = (XmlNameSyntax)VisitXmlName(result);
            }
            return result;
        }

    public override SyntaxNode VisitXmlPrefix(XmlPrefixSyntax node) {
                
            XmlPrefixSyntax result = node;
            foreach (var item in items) {
                result = (XmlPrefixSyntax)VisitXmlPrefix(result);
            }
            return result;
        }

    public override SyntaxNode VisitXmlTextAttribute(XmlTextAttributeSyntax node) {
                
            XmlTextAttributeSyntax result = node;
            foreach (var item in items) {
                result = (XmlTextAttributeSyntax)VisitXmlTextAttribute(result);
            }
            return result;
        }

    public override SyntaxNode VisitXmlCrefAttribute(XmlCrefAttributeSyntax node) {
                
            XmlCrefAttributeSyntax result = node;
            foreach (var item in items) {
                result = (XmlCrefAttributeSyntax)VisitXmlCrefAttribute(result);
            }
            return result;
        }

    public override SyntaxNode VisitXmlNameAttribute(XmlNameAttributeSyntax node) {
                
            XmlNameAttributeSyntax result = node;
            foreach (var item in items) {
                result = (XmlNameAttributeSyntax)VisitXmlNameAttribute(result);
            }
            return result;
        }

    public override SyntaxNode VisitXmlText(XmlTextSyntax node) {
                
            XmlTextSyntax result = node;
            foreach (var item in items) {
                result = (XmlTextSyntax)VisitXmlText(result);
            }
            return result;
        }

    public override SyntaxNode VisitXmlCDataSection(XmlCDataSectionSyntax node) {
                
            XmlCDataSectionSyntax result = node;
            foreach (var item in items) {
                result = (XmlCDataSectionSyntax)VisitXmlCDataSection(result);
            }
            return result;
        }

    public override SyntaxNode VisitXmlProcessingInstruction(XmlProcessingInstructionSyntax node) {
                
            XmlProcessingInstructionSyntax result = node;
            foreach (var item in items) {
                result = (XmlProcessingInstructionSyntax)VisitXmlProcessingInstruction(result);
            }
            return result;
        }

    public override SyntaxNode VisitXmlComment(XmlCommentSyntax node) {
                
            XmlCommentSyntax result = node;
            foreach (var item in items) {
                result = (XmlCommentSyntax)VisitXmlComment(result);
            }
            return result;
        }

    public override SyntaxNode VisitIfDirectiveTrivia(IfDirectiveTriviaSyntax node) {
                
            IfDirectiveTriviaSyntax result = node;
            foreach (var item in items) {
                result = (IfDirectiveTriviaSyntax)VisitIfDirectiveTrivia(result);
            }
            return result;
        }

    public override SyntaxNode VisitElifDirectiveTrivia(ElifDirectiveTriviaSyntax node) {
                
            ElifDirectiveTriviaSyntax result = node;
            foreach (var item in items) {
                result = (ElifDirectiveTriviaSyntax)VisitElifDirectiveTrivia(result);
            }
            return result;
        }

    public override SyntaxNode VisitElseDirectiveTrivia(ElseDirectiveTriviaSyntax node) {
                
            ElseDirectiveTriviaSyntax result = node;
            foreach (var item in items) {
                result = (ElseDirectiveTriviaSyntax)VisitElseDirectiveTrivia(result);
            }
            return result;
        }

    public override SyntaxNode VisitEndIfDirectiveTrivia(EndIfDirectiveTriviaSyntax node) {
                
            EndIfDirectiveTriviaSyntax result = node;
            foreach (var item in items) {
                result = (EndIfDirectiveTriviaSyntax)VisitEndIfDirectiveTrivia(result);
            }
            return result;
        }

    public override SyntaxNode VisitRegionDirectiveTrivia(RegionDirectiveTriviaSyntax node) {
                
            RegionDirectiveTriviaSyntax result = node;
            foreach (var item in items) {
                result = (RegionDirectiveTriviaSyntax)VisitRegionDirectiveTrivia(result);
            }
            return result;
        }

    public override SyntaxNode VisitEndRegionDirectiveTrivia(EndRegionDirectiveTriviaSyntax node) {
                
            EndRegionDirectiveTriviaSyntax result = node;
            foreach (var item in items) {
                result = (EndRegionDirectiveTriviaSyntax)VisitEndRegionDirectiveTrivia(result);
            }
            return result;
        }

    public override SyntaxNode VisitErrorDirectiveTrivia(ErrorDirectiveTriviaSyntax node) {
                
            ErrorDirectiveTriviaSyntax result = node;
            foreach (var item in items) {
                result = (ErrorDirectiveTriviaSyntax)VisitErrorDirectiveTrivia(result);
            }
            return result;
        }

    public override SyntaxNode VisitWarningDirectiveTrivia(WarningDirectiveTriviaSyntax node) {
                
            WarningDirectiveTriviaSyntax result = node;
            foreach (var item in items) {
                result = (WarningDirectiveTriviaSyntax)VisitWarningDirectiveTrivia(result);
            }
            return result;
        }

    public override SyntaxNode VisitBadDirectiveTrivia(BadDirectiveTriviaSyntax node) {
                
            BadDirectiveTriviaSyntax result = node;
            foreach (var item in items) {
                result = (BadDirectiveTriviaSyntax)VisitBadDirectiveTrivia(result);
            }
            return result;
        }

    public override SyntaxNode VisitDefineDirectiveTrivia(DefineDirectiveTriviaSyntax node) {
                
            DefineDirectiveTriviaSyntax result = node;
            foreach (var item in items) {
                result = (DefineDirectiveTriviaSyntax)VisitDefineDirectiveTrivia(result);
            }
            return result;
        }

    public override SyntaxNode VisitUndefDirectiveTrivia(UndefDirectiveTriviaSyntax node) {
                
            UndefDirectiveTriviaSyntax result = node;
            foreach (var item in items) {
                result = (UndefDirectiveTriviaSyntax)VisitUndefDirectiveTrivia(result);
            }
            return result;
        }

    public override SyntaxNode VisitLineDirectiveTrivia(LineDirectiveTriviaSyntax node) {
                
            LineDirectiveTriviaSyntax result = node;
            foreach (var item in items) {
                result = (LineDirectiveTriviaSyntax)VisitLineDirectiveTrivia(result);
            }
            return result;
        }

    public override SyntaxNode VisitPragmaWarningDirectiveTrivia(PragmaWarningDirectiveTriviaSyntax node) {
                
            PragmaWarningDirectiveTriviaSyntax result = node;
            foreach (var item in items) {
                result = (PragmaWarningDirectiveTriviaSyntax)VisitPragmaWarningDirectiveTrivia(result);
            }
            return result;
        }

    public override SyntaxNode VisitPragmaChecksumDirectiveTrivia(PragmaChecksumDirectiveTriviaSyntax node) {
                
            PragmaChecksumDirectiveTriviaSyntax result = node;
            foreach (var item in items) {
                result = (PragmaChecksumDirectiveTriviaSyntax)VisitPragmaChecksumDirectiveTrivia(result);
            }
            return result;
        }

    public override SyntaxNode VisitReferenceDirectiveTrivia(ReferenceDirectiveTriviaSyntax node) {
                
            ReferenceDirectiveTriviaSyntax result = node;
            foreach (var item in items) {
                result = (ReferenceDirectiveTriviaSyntax)VisitReferenceDirectiveTrivia(result);
            }
            return result;
        }

    public override SyntaxNode VisitLoadDirectiveTrivia(LoadDirectiveTriviaSyntax node) {
                
            LoadDirectiveTriviaSyntax result = node;
            foreach (var item in items) {
                result = (LoadDirectiveTriviaSyntax)VisitLoadDirectiveTrivia(result);
            }
            return result;
        }

    public override SyntaxNode VisitShebangDirectiveTrivia(ShebangDirectiveTriviaSyntax node) {
                
            ShebangDirectiveTriviaSyntax result = node;
            foreach (var item in items) {
                result = (ShebangDirectiveTriviaSyntax)VisitShebangDirectiveTrivia(result);
            }
            return result;
        }

    public override SyntaxNode Visit(SyntaxNode node) {
                
            SyntaxNode result = node;
            foreach (var item in items) {
                result = (SyntaxNode)Visit(result);
            }
            return result;
        }

    public override SyntaxToken VisitToken(SyntaxToken token) {
                
            SyntaxToken result = token;
            foreach (var item in items) {
                result = (SyntaxToken)VisitToken(result);
            }
            return result;
        }

    public override SyntaxTrivia VisitTrivia(SyntaxTrivia trivia) {
                
            SyntaxTrivia result = trivia;
            foreach (var item in items) {
                result = (SyntaxTrivia)VisitTrivia(result);
            }
            return result;
        }

    public override SyntaxList<TNode> VisitList<TNode>(SyntaxList<TNode> list) {
                
            SyntaxList<TNode> result = list;
            foreach (var item in items) {
                result = (SyntaxList<TNode>)VisitList(result);
            }
            return result;
        }

    public override TNode VisitListElement<TNode>(TNode node) {
                
            TNode result = node;
            foreach (var item in items) {
                result = (TNode)VisitListElement(result);
            }
            return result;
        }

    public override SeparatedSyntaxList<TNode> VisitList<TNode>(SeparatedSyntaxList<TNode> list) {
                
            SeparatedSyntaxList<TNode> result = list;
            foreach (var item in items) {
                result = (SeparatedSyntaxList<TNode>)VisitList(result);
            }
            return result;
        }

    public override SyntaxToken VisitListSeparator(SyntaxToken separator) {
                
            SyntaxToken result = separator;
            foreach (var item in items) {
                result = (SyntaxToken)VisitListSeparator(result);
            }
            return result;
        }

    public override SyntaxTokenList VisitList(SyntaxTokenList list) {
                
            SyntaxTokenList result = list;
            foreach (var item in items) {
                result = (SyntaxTokenList)VisitList(result);
            }
            return result;
        }

    public override SyntaxTriviaList VisitList(SyntaxTriviaList list) {
                
            SyntaxTriviaList result = list;
            foreach (var item in items) {
                result = (SyntaxTriviaList)VisitList(result);
            }
            return result;
        }

    public override SyntaxTrivia VisitListElement(SyntaxTrivia element) {
                
            SyntaxTrivia result = element;
            foreach (var item in items) {
                result = (SyntaxTrivia)VisitListElement(result);
            }
            return result;
        }

    public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node) {
                
            IdentifierNameSyntax result = node;
            foreach (var item in items) {
                result = (IdentifierNameSyntax)VisitIdentifierName(result);
            }
            return result;
        }

    public override SyntaxNode VisitQualifiedName(QualifiedNameSyntax node) {
                
            QualifiedNameSyntax result = node;
            foreach (var item in items) {
                result = (QualifiedNameSyntax)VisitQualifiedName(result);
            }
            return result;
        }

    public override SyntaxNode VisitGenericName(GenericNameSyntax node) {
                
            GenericNameSyntax result = node;
            foreach (var item in items) {
                result = (GenericNameSyntax)VisitGenericName(result);
            }
            return result;
        }

    public override SyntaxNode VisitTypeArgumentList(TypeArgumentListSyntax node) {
                
            TypeArgumentListSyntax result = node;
            foreach (var item in items) {
                result = (TypeArgumentListSyntax)VisitTypeArgumentList(result);
            }
            return result;
        }

    public override SyntaxNode VisitAliasQualifiedName(AliasQualifiedNameSyntax node) {
                
            AliasQualifiedNameSyntax result = node;
            foreach (var item in items) {
                result = (AliasQualifiedNameSyntax)VisitAliasQualifiedName(result);
            }
            return result;
        }

    public override SyntaxNode VisitPredefinedType(PredefinedTypeSyntax node) {
                
            PredefinedTypeSyntax result = node;
            foreach (var item in items) {
                result = (PredefinedTypeSyntax)VisitPredefinedType(result);
            }
            return result;
        }

    public override SyntaxNode VisitArrayType(ArrayTypeSyntax node) {
                
            ArrayTypeSyntax result = node;
            foreach (var item in items) {
                result = (ArrayTypeSyntax)VisitArrayType(result);
            }
            return result;
        }

    public override SyntaxNode VisitArrayRankSpecifier(ArrayRankSpecifierSyntax node) {
                
            ArrayRankSpecifierSyntax result = node;
            foreach (var item in items) {
                result = (ArrayRankSpecifierSyntax)VisitArrayRankSpecifier(result);
            }
            return result;
        }

    public override SyntaxNode VisitPointerType(PointerTypeSyntax node) {
                
            PointerTypeSyntax result = node;
            foreach (var item in items) {
                result = (PointerTypeSyntax)VisitPointerType(result);
            }
            return result;
        }

    public override SyntaxNode VisitNullableType(NullableTypeSyntax node) {
                
            NullableTypeSyntax result = node;
            foreach (var item in items) {
                result = (NullableTypeSyntax)VisitNullableType(result);
            }
            return result;
        }

    public override SyntaxNode VisitOmittedTypeArgument(OmittedTypeArgumentSyntax node) {
                
            OmittedTypeArgumentSyntax result = node;
            foreach (var item in items) {
                result = (OmittedTypeArgumentSyntax)VisitOmittedTypeArgument(result);
            }
            return result;
        }

    public override SyntaxNode VisitParenthesizedExpression(ParenthesizedExpressionSyntax node) {
                
            ParenthesizedExpressionSyntax result = node;
            foreach (var item in items) {
                result = (ParenthesizedExpressionSyntax)VisitParenthesizedExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitPrefixUnaryExpression(PrefixUnaryExpressionSyntax node) {
                
            PrefixUnaryExpressionSyntax result = node;
            foreach (var item in items) {
                result = (PrefixUnaryExpressionSyntax)VisitPrefixUnaryExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitAwaitExpression(AwaitExpressionSyntax node) {
                
            AwaitExpressionSyntax result = node;
            foreach (var item in items) {
                result = (AwaitExpressionSyntax)VisitAwaitExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitPostfixUnaryExpression(PostfixUnaryExpressionSyntax node) {
                
            PostfixUnaryExpressionSyntax result = node;
            foreach (var item in items) {
                result = (PostfixUnaryExpressionSyntax)VisitPostfixUnaryExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitMemberAccessExpression(MemberAccessExpressionSyntax node) {
                
            MemberAccessExpressionSyntax result = node;
            foreach (var item in items) {
                result = (MemberAccessExpressionSyntax)VisitMemberAccessExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitConditionalAccessExpression(ConditionalAccessExpressionSyntax node) {
                
            ConditionalAccessExpressionSyntax result = node;
            foreach (var item in items) {
                result = (ConditionalAccessExpressionSyntax)VisitConditionalAccessExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitMemberBindingExpression(MemberBindingExpressionSyntax node) {
                
            MemberBindingExpressionSyntax result = node;
            foreach (var item in items) {
                result = (MemberBindingExpressionSyntax)VisitMemberBindingExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitElementBindingExpression(ElementBindingExpressionSyntax node) {
                
            ElementBindingExpressionSyntax result = node;
            foreach (var item in items) {
                result = (ElementBindingExpressionSyntax)VisitElementBindingExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitImplicitElementAccess(ImplicitElementAccessSyntax node) {
                
            ImplicitElementAccessSyntax result = node;
            foreach (var item in items) {
                result = (ImplicitElementAccessSyntax)VisitImplicitElementAccess(result);
            }
            return result;
        }

    public override SyntaxNode VisitBinaryExpression(BinaryExpressionSyntax node) {
                
            BinaryExpressionSyntax result = node;
            foreach (var item in items) {
                result = (BinaryExpressionSyntax)VisitBinaryExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitAssignmentExpression(AssignmentExpressionSyntax node) {
                
            AssignmentExpressionSyntax result = node;
            foreach (var item in items) {
                result = (AssignmentExpressionSyntax)VisitAssignmentExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitConditionalExpression(ConditionalExpressionSyntax node) {
                
            ConditionalExpressionSyntax result = node;
            foreach (var item in items) {
                result = (ConditionalExpressionSyntax)VisitConditionalExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitThisExpression(ThisExpressionSyntax node) {
                
            ThisExpressionSyntax result = node;
            foreach (var item in items) {
                result = (ThisExpressionSyntax)VisitThisExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitBaseExpression(BaseExpressionSyntax node) {
                
            BaseExpressionSyntax result = node;
            foreach (var item in items) {
                result = (BaseExpressionSyntax)VisitBaseExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitLiteralExpression(LiteralExpressionSyntax node) {
                
            LiteralExpressionSyntax result = node;
            foreach (var item in items) {
                result = (LiteralExpressionSyntax)VisitLiteralExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitMakeRefExpression(MakeRefExpressionSyntax node) {
                
            MakeRefExpressionSyntax result = node;
            foreach (var item in items) {
                result = (MakeRefExpressionSyntax)VisitMakeRefExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitRefTypeExpression(RefTypeExpressionSyntax node) {
                
            RefTypeExpressionSyntax result = node;
            foreach (var item in items) {
                result = (RefTypeExpressionSyntax)VisitRefTypeExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitRefValueExpression(RefValueExpressionSyntax node) {
                
            RefValueExpressionSyntax result = node;
            foreach (var item in items) {
                result = (RefValueExpressionSyntax)VisitRefValueExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitCheckedExpression(CheckedExpressionSyntax node) {
                
            CheckedExpressionSyntax result = node;
            foreach (var item in items) {
                result = (CheckedExpressionSyntax)VisitCheckedExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitDefaultExpression(DefaultExpressionSyntax node) {
                
            DefaultExpressionSyntax result = node;
            foreach (var item in items) {
                result = (DefaultExpressionSyntax)VisitDefaultExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitTypeOfExpression(TypeOfExpressionSyntax node) {
                
            TypeOfExpressionSyntax result = node;
            foreach (var item in items) {
                result = (TypeOfExpressionSyntax)VisitTypeOfExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitSizeOfExpression(SizeOfExpressionSyntax node) {
                
            SizeOfExpressionSyntax result = node;
            foreach (var item in items) {
                result = (SizeOfExpressionSyntax)VisitSizeOfExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node) {
                
            InvocationExpressionSyntax result = node;
            foreach (var item in items) {
                result = (InvocationExpressionSyntax)VisitInvocationExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitElementAccessExpression(ElementAccessExpressionSyntax node) {
                
            ElementAccessExpressionSyntax result = node;
            foreach (var item in items) {
                result = (ElementAccessExpressionSyntax)VisitElementAccessExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitArgumentList(ArgumentListSyntax node) {
                
            ArgumentListSyntax result = node;
            foreach (var item in items) {
                result = (ArgumentListSyntax)VisitArgumentList(result);
            }
            return result;
        }

    public override SyntaxNode VisitBracketedArgumentList(BracketedArgumentListSyntax node) {
                
            BracketedArgumentListSyntax result = node;
            foreach (var item in items) {
                result = (BracketedArgumentListSyntax)VisitBracketedArgumentList(result);
            }
            return result;
        }

    public override SyntaxNode VisitArgument(ArgumentSyntax node) {
                
            ArgumentSyntax result = node;
            foreach (var item in items) {
                result = (ArgumentSyntax)VisitArgument(result);
            }
            return result;
        }

    public override SyntaxNode VisitNameColon(NameColonSyntax node) {
                
            NameColonSyntax result = node;
            foreach (var item in items) {
                result = (NameColonSyntax)VisitNameColon(result);
            }
            return result;
        }

    public override SyntaxNode VisitCastExpression(CastExpressionSyntax node) {
                
            CastExpressionSyntax result = node;
            foreach (var item in items) {
                result = (CastExpressionSyntax)VisitCastExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node) {
                
            AnonymousMethodExpressionSyntax result = node;
            foreach (var item in items) {
                result = (AnonymousMethodExpressionSyntax)VisitAnonymousMethodExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node) {
                
            SimpleLambdaExpressionSyntax result = node;
            foreach (var item in items) {
                result = (SimpleLambdaExpressionSyntax)VisitSimpleLambdaExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node) {
                
            ParenthesizedLambdaExpressionSyntax result = node;
            foreach (var item in items) {
                result = (ParenthesizedLambdaExpressionSyntax)VisitParenthesizedLambdaExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitInitializerExpression(InitializerExpressionSyntax node) {
                
            InitializerExpressionSyntax result = node;
            foreach (var item in items) {
                result = (InitializerExpressionSyntax)VisitInitializerExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitObjectCreationExpression(ObjectCreationExpressionSyntax node) {
                
            ObjectCreationExpressionSyntax result = node;
            foreach (var item in items) {
                result = (ObjectCreationExpressionSyntax)VisitObjectCreationExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitAnonymousObjectMemberDeclarator(AnonymousObjectMemberDeclaratorSyntax node) {
                
            AnonymousObjectMemberDeclaratorSyntax result = node;
            foreach (var item in items) {
                result = (AnonymousObjectMemberDeclaratorSyntax)VisitAnonymousObjectMemberDeclarator(result);
            }
            return result;
        }

    public override SyntaxNode VisitAnonymousObjectCreationExpression(AnonymousObjectCreationExpressionSyntax node) {
                
            AnonymousObjectCreationExpressionSyntax result = node;
            foreach (var item in items) {
                result = (AnonymousObjectCreationExpressionSyntax)VisitAnonymousObjectCreationExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitArrayCreationExpression(ArrayCreationExpressionSyntax node) {
                
            ArrayCreationExpressionSyntax result = node;
            foreach (var item in items) {
                result = (ArrayCreationExpressionSyntax)VisitArrayCreationExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitImplicitArrayCreationExpression(ImplicitArrayCreationExpressionSyntax node) {
                
            ImplicitArrayCreationExpressionSyntax result = node;
            foreach (var item in items) {
                result = (ImplicitArrayCreationExpressionSyntax)VisitImplicitArrayCreationExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitStackAllocArrayCreationExpression(StackAllocArrayCreationExpressionSyntax node) {
                
            StackAllocArrayCreationExpressionSyntax result = node;
            foreach (var item in items) {
                result = (StackAllocArrayCreationExpressionSyntax)VisitStackAllocArrayCreationExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitQueryExpression(QueryExpressionSyntax node) {
                
            QueryExpressionSyntax result = node;
            foreach (var item in items) {
                result = (QueryExpressionSyntax)VisitQueryExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitQueryBody(QueryBodySyntax node) {
                
            QueryBodySyntax result = node;
            foreach (var item in items) {
                result = (QueryBodySyntax)VisitQueryBody(result);
            }
            return result;
        }

    public override SyntaxNode VisitFromClause(FromClauseSyntax node) {
                
            FromClauseSyntax result = node;
            foreach (var item in items) {
                result = (FromClauseSyntax)VisitFromClause(result);
            }
            return result;
        }

    public override SyntaxNode VisitLetClause(LetClauseSyntax node) {
                
            LetClauseSyntax result = node;
            foreach (var item in items) {
                result = (LetClauseSyntax)VisitLetClause(result);
            }
            return result;
        }

    public override SyntaxNode VisitJoinClause(JoinClauseSyntax node) {
                
            JoinClauseSyntax result = node;
            foreach (var item in items) {
                result = (JoinClauseSyntax)VisitJoinClause(result);
            }
            return result;
        }

    public override SyntaxNode VisitJoinIntoClause(JoinIntoClauseSyntax node) {
                
            JoinIntoClauseSyntax result = node;
            foreach (var item in items) {
                result = (JoinIntoClauseSyntax)VisitJoinIntoClause(result);
            }
            return result;
        }

    public override SyntaxNode VisitWhereClause(WhereClauseSyntax node) {
                
            WhereClauseSyntax result = node;
            foreach (var item in items) {
                result = (WhereClauseSyntax)VisitWhereClause(result);
            }
            return result;
        }

    public override SyntaxNode VisitOrderByClause(OrderByClauseSyntax node) {
                
            OrderByClauseSyntax result = node;
            foreach (var item in items) {
                result = (OrderByClauseSyntax)VisitOrderByClause(result);
            }
            return result;
        }

    public override SyntaxNode VisitOrdering(OrderingSyntax node) {
                
            OrderingSyntax result = node;
            foreach (var item in items) {
                result = (OrderingSyntax)VisitOrdering(result);
            }
            return result;
        }

    public override SyntaxNode VisitSelectClause(SelectClauseSyntax node) {
                
            SelectClauseSyntax result = node;
            foreach (var item in items) {
                result = (SelectClauseSyntax)VisitSelectClause(result);
            }
            return result;
        }

    public override SyntaxNode VisitGroupClause(GroupClauseSyntax node) {
                
            GroupClauseSyntax result = node;
            foreach (var item in items) {
                result = (GroupClauseSyntax)VisitGroupClause(result);
            }
            return result;
        }

    public override SyntaxNode VisitQueryContinuation(QueryContinuationSyntax node) {
                
            QueryContinuationSyntax result = node;
            foreach (var item in items) {
                result = (QueryContinuationSyntax)VisitQueryContinuation(result);
            }
            return result;
        }

    public override SyntaxNode VisitOmittedArraySizeExpression(OmittedArraySizeExpressionSyntax node) {
                
            OmittedArraySizeExpressionSyntax result = node;
            foreach (var item in items) {
                result = (OmittedArraySizeExpressionSyntax)VisitOmittedArraySizeExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitInterpolatedStringExpression(InterpolatedStringExpressionSyntax node) {
                
            InterpolatedStringExpressionSyntax result = node;
            foreach (var item in items) {
                result = (InterpolatedStringExpressionSyntax)VisitInterpolatedStringExpression(result);
            }
            return result;
        }

    public override SyntaxNode VisitInterpolatedStringText(InterpolatedStringTextSyntax node) {
                
            InterpolatedStringTextSyntax result = node;
            foreach (var item in items) {
                result = (InterpolatedStringTextSyntax)VisitInterpolatedStringText(result);
            }
            return result;
        }

    public override SyntaxNode VisitInterpolation(InterpolationSyntax node) {
                
            InterpolationSyntax result = node;
            foreach (var item in items) {
                result = (InterpolationSyntax)VisitInterpolation(result);
            }
            return result;
        }

    public override SyntaxNode VisitInterpolationAlignmentClause(InterpolationAlignmentClauseSyntax node) {
                
            InterpolationAlignmentClauseSyntax result = node;
            foreach (var item in items) {
                result = (InterpolationAlignmentClauseSyntax)VisitInterpolationAlignmentClause(result);
            }
            return result;
        }

    public override SyntaxNode VisitInterpolationFormatClause(InterpolationFormatClauseSyntax node) {
                
            InterpolationFormatClauseSyntax result = node;
            foreach (var item in items) {
                result = (InterpolationFormatClauseSyntax)VisitInterpolationFormatClause(result);
            }
            return result;
        }

    public override SyntaxNode VisitGlobalStatement(GlobalStatementSyntax node) {
                
            GlobalStatementSyntax result = node;
            foreach (var item in items) {
                result = (GlobalStatementSyntax)VisitGlobalStatement(result);
            }
            return result;
        }

    public override SyntaxNode VisitBlock(BlockSyntax node) {
                
            BlockSyntax result = node;
            foreach (var item in items) {
                result = (BlockSyntax)VisitBlock(result);
            }
            return result;
        }

    public override SyntaxNode VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node) {
                
            LocalDeclarationStatementSyntax result = node;
            foreach (var item in items) {
                result = (LocalDeclarationStatementSyntax)VisitLocalDeclarationStatement(result);
            }
            return result;
        }

    public override SyntaxNode VisitVariableDeclaration(VariableDeclarationSyntax node) {
                
            VariableDeclarationSyntax result = node;
            foreach (var item in items) {
                result = (VariableDeclarationSyntax)VisitVariableDeclaration(result);
            }
            return result;
        }

    public override SyntaxNode VisitVariableDeclarator(VariableDeclaratorSyntax node) {
                
            VariableDeclaratorSyntax result = node;
            foreach (var item in items) {
                result = (VariableDeclaratorSyntax)VisitVariableDeclarator(result);
            }
            return result;
        }

    public override SyntaxNode VisitEqualsValueClause(EqualsValueClauseSyntax node) {
                
            EqualsValueClauseSyntax result = node;
            foreach (var item in items) {
                result = (EqualsValueClauseSyntax)VisitEqualsValueClause(result);
            }
            return result;
        }

    public override SyntaxNode VisitExpressionStatement(ExpressionStatementSyntax node) {
                
            ExpressionStatementSyntax result = node;
            foreach (var item in items) {
                result = (ExpressionStatementSyntax)VisitExpressionStatement(result);
            }
            return result;
        }

    public override SyntaxNode VisitEmptyStatement(EmptyStatementSyntax node) {
                
            EmptyStatementSyntax result = node;
            foreach (var item in items) {
                result = (EmptyStatementSyntax)VisitEmptyStatement(result);
            }
            return result;
        }

    public override SyntaxNode VisitLabeledStatement(LabeledStatementSyntax node) {
                
            LabeledStatementSyntax result = node;
            foreach (var item in items) {
                result = (LabeledStatementSyntax)VisitLabeledStatement(result);
            }
            return result;
        }

    public override SyntaxNode VisitGotoStatement(GotoStatementSyntax node) {
                
            GotoStatementSyntax result = node;
            foreach (var item in items) {
                result = (GotoStatementSyntax)VisitGotoStatement(result);
            }
            return result;
        }

    public override SyntaxNode VisitBreakStatement(BreakStatementSyntax node) {
                
            BreakStatementSyntax result = node;
            foreach (var item in items) {
                result = (BreakStatementSyntax)VisitBreakStatement(result);
            }
            return result;
        }

    public override SyntaxNode VisitContinueStatement(ContinueStatementSyntax node) {
                
            ContinueStatementSyntax result = node;
            foreach (var item in items) {
                result = (ContinueStatementSyntax)VisitContinueStatement(result);
            }
            return result;
        }

    public override SyntaxNode VisitReturnStatement(ReturnStatementSyntax node) {
                
            ReturnStatementSyntax result = node;
            foreach (var item in items) {
                result = (ReturnStatementSyntax)VisitReturnStatement(result);
            }
            return result;
        }

    public override SyntaxNode VisitThrowStatement(ThrowStatementSyntax node) {
                
            ThrowStatementSyntax result = node;
            foreach (var item in items) {
                result = (ThrowStatementSyntax)VisitThrowStatement(result);
            }
            return result;
        }

    public override SyntaxNode VisitYieldStatement(YieldStatementSyntax node) {
                
            YieldStatementSyntax result = node;
            foreach (var item in items) {
                result = (YieldStatementSyntax)VisitYieldStatement(result);
            }
            return result;
        }

    public override SyntaxNode VisitWhileStatement(WhileStatementSyntax node) {
                
            WhileStatementSyntax result = node;
            foreach (var item in items) {
                result = (WhileStatementSyntax)VisitWhileStatement(result);
            }
            return result;
        }

    public override SyntaxNode VisitDoStatement(DoStatementSyntax node) {
                
            DoStatementSyntax result = node;
            foreach (var item in items) {
                result = (DoStatementSyntax)VisitDoStatement(result);
            }
            return result;
        }

    public override SyntaxNode VisitForStatement(ForStatementSyntax node) {
                
            ForStatementSyntax result = node;
            foreach (var item in items) {
                result = (ForStatementSyntax)VisitForStatement(result);
            }
            return result;
        }

    public override SyntaxNode VisitForEachStatement(ForEachStatementSyntax node) {
                
            ForEachStatementSyntax result = node;
            foreach (var item in items) {
                result = (ForEachStatementSyntax)VisitForEachStatement(result);
            }
            return result;
        }

    public override SyntaxNode VisitUsingStatement(UsingStatementSyntax node) {
                
            UsingStatementSyntax result = node;
            foreach (var item in items) {
                result = (UsingStatementSyntax)VisitUsingStatement(result);
            }
            return result;
        }

    public override SyntaxNode VisitFixedStatement(FixedStatementSyntax node) {
                
            FixedStatementSyntax result = node;
            foreach (var item in items) {
                result = (FixedStatementSyntax)VisitFixedStatement(result);
            }
            return result;
        }

    public override SyntaxNode VisitCheckedStatement(CheckedStatementSyntax node) {
                
            CheckedStatementSyntax result = node;
            foreach (var item in items) {
                result = (CheckedStatementSyntax)VisitCheckedStatement(result);
            }
            return result;
        }

    public override SyntaxNode VisitUnsafeStatement(UnsafeStatementSyntax node) {
                
            UnsafeStatementSyntax result = node;
            foreach (var item in items) {
                result = (UnsafeStatementSyntax)VisitUnsafeStatement(result);
            }
            return result;
        }

    public override SyntaxNode VisitLockStatement(LockStatementSyntax node) {
                
            LockStatementSyntax result = node;
            foreach (var item in items) {
                result = (LockStatementSyntax)VisitLockStatement(result);
            }
            return result;
        }

    public override SyntaxNode VisitIfStatement(IfStatementSyntax node) {
                
            IfStatementSyntax result = node;
            foreach (var item in items) {
                result = (IfStatementSyntax)VisitIfStatement(result);
            }
            return result;
        }

    public override SyntaxNode VisitElseClause(ElseClauseSyntax node) {
                
            ElseClauseSyntax result = node;
            foreach (var item in items) {
                result = (ElseClauseSyntax)VisitElseClause(result);
            }
            return result;
        }

    public override SyntaxNode VisitSwitchStatement(SwitchStatementSyntax node) {
                
            SwitchStatementSyntax result = node;
            foreach (var item in items) {
                result = (SwitchStatementSyntax)VisitSwitchStatement(result);
            }
            return result;
        }

    public override SyntaxNode VisitSwitchSection(SwitchSectionSyntax node) {
                
            SwitchSectionSyntax result = node;
            foreach (var item in items) {
                result = (SwitchSectionSyntax)VisitSwitchSection(result);
            }
            return result;
        }

    public override SyntaxNode VisitCaseSwitchLabel(CaseSwitchLabelSyntax node) {
                
            CaseSwitchLabelSyntax result = node;
            foreach (var item in items) {
                result = (CaseSwitchLabelSyntax)VisitCaseSwitchLabel(result);
            }
            return result;
        }

    public override SyntaxNode VisitDefaultSwitchLabel(DefaultSwitchLabelSyntax node) {
                
            DefaultSwitchLabelSyntax result = node;
            foreach (var item in items) {
                result = (DefaultSwitchLabelSyntax)VisitDefaultSwitchLabel(result);
            }
            return result;
        }

    public override SyntaxNode VisitTryStatement(TryStatementSyntax node) {
                
            TryStatementSyntax result = node;
            foreach (var item in items) {
                result = (TryStatementSyntax)VisitTryStatement(result);
            }
            return result;
        }

    public override SyntaxNode VisitCatchClause(CatchClauseSyntax node) {
                
            CatchClauseSyntax result = node;
            foreach (var item in items) {
                result = (CatchClauseSyntax)VisitCatchClause(result);
            }
            return result;
        }

    public override SyntaxNode VisitCatchDeclaration(CatchDeclarationSyntax node) {
                
            CatchDeclarationSyntax result = node;
            foreach (var item in items) {
                result = (CatchDeclarationSyntax)VisitCatchDeclaration(result);
            }
            return result;
        }

    public override SyntaxNode VisitCatchFilterClause(CatchFilterClauseSyntax node) {
                
            CatchFilterClauseSyntax result = node;
            foreach (var item in items) {
                result = (CatchFilterClauseSyntax)VisitCatchFilterClause(result);
            }
            return result;
        }

    public override SyntaxNode VisitFinallyClause(FinallyClauseSyntax node) {
                
            FinallyClauseSyntax result = node;
            foreach (var item in items) {
                result = (FinallyClauseSyntax)VisitFinallyClause(result);
            }
            return result;
        }

    public override SyntaxNode VisitCompilationUnit(CompilationUnitSyntax node) {
                
            CompilationUnitSyntax result = node;
            foreach (var item in items) {
                result = (CompilationUnitSyntax)VisitCompilationUnit(result);
            }
            return result;
        }

    public override SyntaxNode VisitExternAliasDirective(ExternAliasDirectiveSyntax node) {
                
            ExternAliasDirectiveSyntax result = node;
            foreach (var item in items) {
                result = (ExternAliasDirectiveSyntax)VisitExternAliasDirective(result);
            }
            return result;
        }

    public override SyntaxNode VisitUsingDirective(UsingDirectiveSyntax node) {
                
            UsingDirectiveSyntax result = node;
            foreach (var item in items) {
                result = (UsingDirectiveSyntax)VisitUsingDirective(result);
            }
            return result;
        }

    public override SyntaxNode VisitNamespaceDeclaration(NamespaceDeclarationSyntax node) {
                
            NamespaceDeclarationSyntax result = node;
            foreach (var item in items) {
                result = (NamespaceDeclarationSyntax)VisitNamespaceDeclaration(result);
            }
            return result;
        }

    public override SyntaxNode VisitAttributeList(AttributeListSyntax node) {
                
            AttributeListSyntax result = node;
            foreach (var item in items) {
                result = (AttributeListSyntax)VisitAttributeList(result);
            }
            return result;
        }

    public override SyntaxNode VisitAttributeTargetSpecifier(AttributeTargetSpecifierSyntax node) {
                
            AttributeTargetSpecifierSyntax result = node;
            foreach (var item in items) {
                result = (AttributeTargetSpecifierSyntax)VisitAttributeTargetSpecifier(result);
            }
            return result;
        }

    public override SyntaxNode VisitAttribute(AttributeSyntax node) {
                
            AttributeSyntax result = node;
            foreach (var item in items) {
                result = (AttributeSyntax)VisitAttribute(result);
            }
            return result;
        }

    public override SyntaxNode VisitAttributeArgumentList(AttributeArgumentListSyntax node) {
                
            AttributeArgumentListSyntax result = node;
            foreach (var item in items) {
                result = (AttributeArgumentListSyntax)VisitAttributeArgumentList(result);
            }
            return result;
        }

    public override SyntaxNode DefaultVisit(SyntaxNode node) {
                
            SyntaxNode result = node;
            foreach (var item in items) {
                result = (SyntaxNode)DefaultVisit(result);
            }
            return result;
        }


    }
}

