%% initialize
candidate=round(rand(15,3)*256,0)-1;
objective=round(rand(1,3)*256,0)-1;
%objective=[128 128 128];
eucl=ones(size(candidate,1),1);
iter=0;
[B, I]=mink(eucl,5);
best=candidate(I(1),:);
rec=[0 0 0];
precision=10;
figure1 = figure;
%% begin selection, set terminal conditions
while abs(best(1)-objective(1))>precision ||...
      abs(best(2)-objective(2))>precision ||...
      abs(best(3)-objective(3))>precision %min(eucl)~=0
%% calculate euclidean distance
for i=1:size(candidate,1)
eucl(i) = sqrt(sum((candidate(i,:) - objective) .^ 2));
end
%% histogram
histo=1000-eucl;
%% select best 5 solutions
[B, I]=mink(eucl,5);
best=candidate(I(1),:);
ind=zeros(size(candidate,1),1);
ind(I)=1;
kill=candidate.*ind;
candidate=sortrows(kill,'descend');
%% new generation by full pairwise combination
for i=1:3
candidate(6,i)=round(mean([candidate(1,i) candidate(2,i)]),0);
end
for i=1:3
candidate(7,i)=round(mean([candidate(1,i) candidate(3,i)]),0);
end
for i=1:3
candidate(8,i)=round(mean([candidate(1,i) candidate(4,i)]),0);
end
for i=1:3
candidate(9,i)=round(mean([candidate(1,i) candidate(5,i)]),0);
end
for i=1:3
candidate(10,i)=round(mean([candidate(2,i) candidate(3,i)]),0);
end
for i=1:3
candidate(11,i)=round(mean([candidate(2,i) candidate(4,i)]),0);
end
for i=1:3
candidate(12,i)=round(mean([candidate(2,i) candidate(5,i)]),0);
end
for i=1:3
candidate(13,i)=round(mean([candidate(3,i) candidate(4,i)]),0);
end
for i=1:3
candidate(14,i)=round(mean([candidate(3,i) candidate(5,i)]),0);
end
for i=1:3
candidate(15,i)=round(mean([candidate(4,i) candidate(5,i)]),0);
end
%% maintanance 
iter=iter+1;
message=['Best match = ', num2str(best), ', iteration: ', num2str(iter),' , objective =',num2str(objective)];
%message
rec=[rec;best];
%% mutation, if stuck in same solution 
if rec(iter+1,:)==rec(iter,:)
   mutate=(rand(3,1).*30)-15;
   for i=1:size(candidate,1)
   candidate(i,:)=round(bsxfun(@plus,candidate(i,:),mutate'),0);
   end
end
%% capping, if any is <0, >255
    for i=1:45
       if candidate(i)<0
           candidate(i)=0;
       elseif candidate(i)>255
           candidate(i)=255;
       end
    end
%% chart
    figure(figure1);
    axes1 = axes('Parent',figure1);
    %axes1=subplot(2,1,1);
    hold(axes1,'on');
    plot(rec, 'black','Marker','.');
    xlim(axes1,[1 size(rec,1)]);
    ylim(axes1,[0 255]);
    annotation('textbox',[.15 .6 .3 .3],'String',{message},'FontSize',12,'FitBoxToText','on', 'BackgroundColor','white');
    line([1 size(rec,1)], [objective(1) objective(1)], 'Color','red','LineStyle','--');
    line([1 size(rec,1)], [objective(2) objective(2)], 'Color','green','LineStyle','--');
    line([1 size(rec,1)], [objective(3) objective(3)], 'Color','blue','LineStyle','--');
    linkdata on;
    drawnow;
    
    %axes2 = axes('Parent',figure1);
    %axes2=subplot(2,1,2);
    %hold(axes2,'on');
        
%     bar(histo);
%     box(axes2,'on');
%     set(axes2,'XTick',[1 2 3 4 5 6 7 8 9 10 11 12 13 14 15]);
%     linkdata on;
%     drawnow;    
end